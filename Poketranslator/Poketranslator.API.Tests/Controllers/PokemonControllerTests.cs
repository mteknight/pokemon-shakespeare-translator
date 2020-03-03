using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
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
            const HttpStatusCode expectedBadRequest = HttpStatusCode.BadRequest;
            var pokemonTranslationServiceMock = new Mock<IPokemonTranslationService>();
            var sutController = new PokemonController(pokemonTranslationServiceMock.Object);
            sutController.Request = new HttpRequestMessage();
            sutController.Configuration = new HttpConfiguration();

            // Act
            var response = await sutController.Get(pokemonName, CancellationToken.None);
            var responseMessage = await response.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(responseMessage);
            Assert.Equal(expectedBadRequest, responseMessage.StatusCode);
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
            sutController.Request = new HttpRequestMessage();
            sutController.Configuration = new HttpConfiguration();

            // Act
            var response = await sutController.Get(pokemonName, CancellationToken.None);
            var responseMessage = await response.ExecuteAsync(CancellationToken.None);
            responseMessage.EnsureSuccessStatusCode();

            var canGetContent = responseMessage.TryGetContentValue(out PokemonModel pokemonModel);

            // Assert
            Assert.True(canGetContent);
            Assert.NotNull(pokemonModel);
            Assert.Equal(expectedPokemonModel, pokemonModel, new PokemonModelComparer());
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
            const HttpStatusCode expectedNotFound = HttpStatusCode.NotFound;
            SetupPokemonTranslationServiceMock(pokemonTranslationServiceMock, default);
            var sutController = new PokemonController(pokemonTranslationServiceMock.Object);
            sutController.Request = new HttpRequestMessage();
            sutController.Configuration = new HttpConfiguration();

            // Act
            var response = await sutController.Get(pokemonName, CancellationToken.None);
            var responseMessage = await response.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(responseMessage);
            Assert.Equal(expectedNotFound, responseMessage.StatusCode);
        }
    }
}
