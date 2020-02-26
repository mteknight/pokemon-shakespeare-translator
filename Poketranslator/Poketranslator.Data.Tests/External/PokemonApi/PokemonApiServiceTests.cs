using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Crosscutting.DependencyInjection;
using Poketranslator.Data.Interfaces.External.PokemonApi;
using Xunit;

namespace Poketranslator.Data.Tests.External.PokemonApi
{
    public class PokemonApiServiceTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        public Task TestGetByName_WhenPokemonNameIsInvalid_ExpectArgumentNullException(string pokemonName)
        {
            // Arrange
            var services = new ServiceCollection();
            services.ConfigureDataDependencies();
            var provider = services.BuildServiceProvider();

            var sutService = provider.GetService<IPokemonApiService>();

            // Act
            Func<Task> SutCall() => () => sutService.GetByName(pokemonName);

            // Assert
            return Assert.ThrowsAsync<ArgumentNullException>(SutCall());
        }
    }
}
