using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Crosscutting.DependencyInjection;
using Poketranslator.Data.Interfaces.Services;
using Xunit;

namespace PokeTranslator.Data.IntegrationTests.Services
{
    public class ShakespeareApiServiceIntegrationTests
    {
        private readonly IServiceCollection _services;

        public ShakespeareApiServiceIntegrationTests()
        {
            _services = new ServiceCollection();
            _services.ConfigureDataDependencies();
        }

        [Theory]
        [MemberData(nameof(GetShakespeareTestData))]
        public async Task TestGetTranslation_WithValidPokemonDescription_ExpectShakespeareanTranslation(
            string pokemonDescription,
            string expectedTranslation)
        {
            // Arrange
            var provider = _services.BuildServiceProvider();
            var sutService = provider.GetService<IShakespeareTranslationService>();

            // Act
            var translation = await sutService.GetTranslation(pokemonDescription, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.NotNull(translation);
            Assert.Equal(expectedTranslation, translation);
        }

        public static IEnumerable<object[]> GetShakespeareTestData()
        {
            yield return new[]
            {
                "Charizard flies around the sky in search of powerful opponents. It breathes fire of such great heat that it melts anything. However, it never turns its fiery breath on any opponent weaker than itself.",
                "Charizard flies 'round the sky in search of powerful opponents. 't breathes fire of such most wondrous heat yond 't melts aught. However, 't nev'r turns its fiery breath on any opponent weaker than itself."
            };
        }
    }
}
