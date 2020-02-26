using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Data.External.PokemonApi;
using Poketranslator.Data.Interfaces.External.PokemonApi;

namespace Poketranslator.Crosscutting.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDataDependencies(this IServiceCollection services)
        {
            return services
                .AddSingleton<IPokemonApiService, PokemonApiService>();
        }
    }
}
