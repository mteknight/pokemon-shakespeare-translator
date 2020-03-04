using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poketranslator.API.Controllers;
using Poketranslator.Domain.Interfaces.Models;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Models;
using Poketranslator.Domain.Services;
using Poketranslator.Tests.Common.Helpers;
using Xunit;

namespace Poketranslator.API.AcceptanceTests.Controllers
{
    public class PokemonControllerAcceptanceTests
    {
        [Theory]
        [MemberData(nameof(GetPokemonTranslationAcceptanceTestData))]
        public async Task TestPokemonTranslation_WhenSuccessful_ExpectTranslatedModel(
            IPokemonModel expectedPokemonModel)
        {
            // Arrange
            var pokemonTranslationService = DIHelper.GetServices()
                .GetConfiguredService<IPokemonTranslationService>();

            var sutController = new PokemonController(pokemonTranslationService);

            // Act
            var actionResult = await sutController.Get(expectedPokemonModel.Name, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var translatedPokemon = Assert.IsType<PokemonModel>(objectResult.Value);
            Assert.Equal(expectedPokemonModel, translatedPokemon, new PokemonModelComparer());
        }

        public static IEnumerable<object[]> GetPokemonTranslationAcceptanceTestData()
        {
            var pokemonModel = new PokemonModel
            {
                Name = "charizard",
                Description = "Charizard flies 'round the sky in search of powerful opponents. 't breathes fire of such most wondrous heat yond 't melts aught. However, 't nev'r turns its fiery breath on any opponent weaker than itself."
            };

            yield return new[] {pokemonModel};
        }
    }
}
