using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Domain.Interfaces.Domain;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Models;
using Poketranslator.Domain.Services;
using Poketranslator.Tests.Common.Helpers;
using Xunit;

namespace Poketranslator.Domain.Tests.Services
{
    public class PokemonTranslationServiceTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        [InlineData(" ")]
        public Task TestTranslate_WhenPokemonNameIsInvalid_ExpectArgumentNullException(
            string pokemonName)
        {
            // Arrange
            var sutService = DIHelper.GetServices()
                .GetConfiguredService<IPokemonTranslationService>();

            // Act
            Func<Task> SutCall() => () => sutService.Translate(pokemonName, CancellationToken.None);

            // Assert
            return Assert.ThrowsAsync<ArgumentNullException>(SutCall());
        }

        [Theory]
        [AutoMoqData]
        public async Task TestTranslate_WithValidPokemonName_ExpectPokemonWithTranslatedDescription(
            Mock<IPokemonApiService> pokemonApiServiceMock,
            Mock<IShakespeareTranslationService> shakespeareTranslationServiceMock,
            string pokemonName,
            IPokemon pokemon)
        {
            // Arrange
            var expectedPokemonModel = new PokemonModel
            {
                Name = pokemon.Name,
                Description = pokemon.OriginalDescription
            };

            SetupPokemonApiServiceMock(pokemonApiServiceMock, pokemon);
            SetupShakespeareTranslationServiceMock(shakespeareTranslationServiceMock, pokemon);

            var sutService = DIHelper.GetServices()
                .RegisterMock(pokemonApiServiceMock)
                .RegisterMock(shakespeareTranslationServiceMock)
                .GetConfiguredService<IPokemonTranslationService>();

            // Act
            var translatedPokemon = await sutService.Translate(pokemonName, CancellationToken.None);

            // Assert
            Assert.NotNull(translatedPokemon);
            Assert.Equal(expectedPokemonModel, translatedPokemon, new PokemonModelComparer());
        }

        private static void SetupPokemonApiServiceMock(
            Mock<IPokemonApiService> pokemonApiServiceMock,
            IPokemon expectedPokemon)
        {
            pokemonApiServiceMock
                .Setup(service => service.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expectedPokemon);
        }

        private static void SetupShakespeareTranslationServiceMock(
            Mock<IShakespeareTranslationService> shakespeareTranslationServiceMock,
            IPokemon expectedPokemon)
        {
            shakespeareTranslationServiceMock
                .Setup(service => service.GetTranslation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expectedPokemon.Translation);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestTranslate_WhenPokemonNotFound_ExpectNullReturn(
            Mock<IPokemonApiService> pokemonApiServiceMock,
            string pokemonName)
        {
            // Arrange
            SetupPokemonApiServiceMock(pokemonApiServiceMock, default(IPokemon));
            var sutService = DIHelper.GetServices()
                .RegisterMock(pokemonApiServiceMock)
                .GetConfiguredService<IPokemonTranslationService>();

            // Act
            var pokemon = await sutService.Translate(pokemonName, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            Assert.Null(pokemon);
        }
    }
}
