using Microsoft.Extensions.DependencyInjection;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Interfaces.Wrappers;
using Poketranslator.Data.Services;
using Poketranslator.Data.Wrappers;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Services;

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

        public static IServiceCollection ConfigureDomainDependencies(this IServiceCollection services)
        {
            return services
                .AddSingleton<IPokemonTranslationService, PokemonTranslationService>();
        }
    }
}
