using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Interfaces.Wrappers;
using Poketranslator.Data.Services;
using Poketranslator.Data.Wrappers;

namespace Poketranslator.Crosscutting.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDataDependencies(this IServiceCollection services)
        {
            return services
                .AddHttpClient()
                .AddSingleton<IHttpClientWrapper, HttpClientWrapper>()
                .AddSingleton<IShakespeareTranslationService, ShakespeareTranslationService>()
                .AddSingleton<IPokeApiClientWrapper, PokeApiClientWrapper>()
                .AddSingleton<IPokemonApiService, PokemonApiService>();
        }
    }
}
