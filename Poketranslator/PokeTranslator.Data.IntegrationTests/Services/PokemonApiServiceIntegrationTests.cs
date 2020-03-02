using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Crosscutting.DependencyInjection;
using Poketranslator.Data.Interfaces.Services;
using Xunit;

namespace PokeTranslator.Data.IntegrationTests.Services
{
    public class PokemonApiServiceIntegrationTests
    {
        private readonly IServiceCollection _services;

        public PokemonApiServiceIntegrationTests()
        {
            _services = new ServiceCollection();
            _services.ConfigureDataDependencies();
        }

        [Theory]
        [MemberData(nameof(GetPokemonTestData))]
        public async Task TestGetByName_WithValidPokemonName_ExpectPokemonWithDescription(
            string pokemonName,
            string expectedDescription)
        {
            // Arrange
            var provider = _services.BuildServiceProvider();
            var sutService = provider.GetService<IPokemonApiService>();

            // Act
            var pokemon = await sutService.GetByName(pokemonName, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            Assert.NotNull(pokemon);
            Assert.Equal(pokemonName, pokemon.Name);
            Assert.Equal(expectedDescription, pokemon.OriginalDescription);
        }

        public static IEnumerable<object[]> GetPokemonTestData()
        {
            yield return new[]
            {
                "charizard",
                "Charizard flies around the sky in search of powerful opponents. It breathes fire of such great heat that it melts anything. However, it never turns its fiery breath on any opponent weaker than itself."
            };

            yield return new[]
            {
                "pikachu",
                "Its nature is to store up electricity. Forests where nests of Pikachu live are dangerous, since the trees are so often struck by lightning."
            };
        }
    }
}
