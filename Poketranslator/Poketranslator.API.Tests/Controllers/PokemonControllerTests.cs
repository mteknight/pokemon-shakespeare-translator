using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Poketranslator.API.Controllers;
using Poketranslator.Domain.Interfaces.Models;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Models;
using Poketranslator.Domain.Services;
using Poketranslator.Tests.Common.Helpers;
using Xunit;

namespace Poketranslator.API.Tests.Controllers
{
    public class PokemonControllerTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TestGet_WhenPokemonNameIsInvalid_ExpectBadRequest(
            string pokemonName)
        {
            // Arrange
            const int expectedBadRequest = StatusCodes.Status400BadRequest;
            var pokemonTranslationServiceMock = new Mock<IPokemonTranslationService>();
            var sutController = new PokemonController(pokemonTranslationServiceMock.Object);

            // Act
            var result = await sutController.Get(pokemonName, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(expectedBadRequest, sutController.Response.StatusCode);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGet_WhenPokemonNameIsValid_ExpectTranslatedPokemon(
            Mock<IPokemonTranslationService> pokemonTranslationServiceMock,
            string pokemonName,
            PokemonModel expectedPokemonModel)
        {
            // Arrange
            SetupPokemonTranslationServiceMock(pokemonTranslationServiceMock, expectedPokemonModel);
            var sutController = new PokemonController(pokemonTranslationServiceMock.Object);

            // Act
            var result = await sutController.Get(pokemonName, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedPokemonModel, result.Value, new PokemonModelComparer());
        }

        private static void SetupPokemonTranslationServiceMock(
            Mock<IPokemonTranslationService> pokemonTranslationServiceMock,
            IPokemonModel expectedPokemonModel)
        {
            pokemonTranslationServiceMock
                .Setup(service => service.Translate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => expectedPokemonModel);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGet_WhenPokemonNotFound_ExpectNotFound(
            Mock<IPokemonTranslationService> pokemonTranslationServiceMock,
            string pokemonName)
        {
            // Arrange
            const int expectedNotFound = StatusCodes.Status404NotFound;
            SetupPokemonTranslationServiceMock(pokemonTranslationServiceMock, default);
            var sutController = new PokemonController(pokemonTranslationServiceMock.Object);

            // Act
            var result = await sutController.Get(pokemonName, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNotFound, sutController.Response.StatusCode);
        }
    }
}
