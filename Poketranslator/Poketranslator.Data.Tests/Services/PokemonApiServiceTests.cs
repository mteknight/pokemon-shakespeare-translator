using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PokeApiNet;
using Poketranslator.Crosscutting.DependencyInjection;
using Poketranslator.Data.Interfaces.External.PokemonApi;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Tests.Helpers;
using Xunit;

namespace Poketranslator.Data.Tests.Services
{
    public class PokemonApiServiceTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        public Task TestGetByName_WhenPokemonNameIsInvalid_ExpectArgumentNullException(string pokemonName)
        {
            // Arrange
            var sutService = DIHelper.GetServices()
                .GetConfiguredService<IPokemonApiService>();

            // Act
            Func<Task> SutCall() => () => sutService.GetByName(pokemonName);

            // Assert
            return Assert.ThrowsAsync<ArgumentNullException>(SutCall());
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGetByName_WhenPokemonNotFound_ExpectNull(
            Mock<IPokeApiClientWrapper> pokeApiClientWrapperMock)
        {
            // Arrange
            const string pokemonName = "kjrsfghdkjas";
            SetupPokeApiClientWrapperMock(pokeApiClientWrapperMock);
            var sutService = DIHelper.GetServices()
                .RegisterMock(pokeApiClientWrapperMock)
                .GetConfiguredService<IPokemonApiService>();

            // Act
            var pokemon = await sutService.GetByName(pokemonName).ConfigureAwait(false);

            // Assert
            Assert.Null(pokemon);
        }

        private static void SetupPokeApiClientWrapperMock(
            Mock<IPokeApiClientWrapper> pokeApiClientWrapperMock,
            PokemonSpecies pokemonSpecies = default)
        {
            pokeApiClientWrapperMock
                .Setup(wrapper => wrapper.GetResourceAsync<PokemonSpecies>(It.IsAny<string>()))
                .ReturnsAsync(() => pokemonSpecies);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGetByName_WithValidPokemonName_ExpectPokemonWithDescription(
            Mock<IPokeApiClientWrapper> pokeApiClientWrapperMock,
            string pokemonName,
            string expectedDescription)
        {
            var pokemonSpecies = PokemonSpeciesHelper.CreatePokemonSpecies(pokemonName, expectedDescription, "en");
            SetupPokeApiClientWrapperMock(pokeApiClientWrapperMock, pokemonSpecies);
            var sutService = DIHelper.GetServices()
                .RegisterMock(pokeApiClientWrapperMock)
                .GetConfiguredService<IPokemonApiService>();

            // Act
            var pokemon = await sutService.GetByName(pokemonName).ConfigureAwait(false);

            // Assert
            Assert.NotNull(pokemon);
            Assert.Equal(pokemonName, pokemon.Name);
            Assert.Equal(expectedDescription, pokemon.OriginalDescription);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGetByName_WithDescriptionContainingLineBreaks_ExpectParsedDescription(
            Mock<IPokeApiClientWrapper> pokeApiClientWrapperMock,
            string pokemonName)
        {
            // Arrange
            const string originalDescription = "expected\ndescription\nwith some\nline breaks.";
            const string expectedDescription = "expected description with some line breaks.";

            var pokemonSpecies = PokemonSpeciesHelper.CreatePokemonSpecies(pokemonName, originalDescription, "en");
            SetupPokeApiClientWrapperMock(pokeApiClientWrapperMock, pokemonSpecies);
            var sutService = DIHelper.GetServices()
                .RegisterMock(pokeApiClientWrapperMock)
                .GetConfiguredService<IPokemonApiService>();

            // Act
            var pokemon = await sutService.GetByName(pokemonName).ConfigureAwait(false);

            // Assert
            Assert.NotNull(pokemon);
            Assert.Equal(pokemonName, pokemon.Name);
            Assert.Equal(expectedDescription, pokemon.OriginalDescription);
        }
    }
}