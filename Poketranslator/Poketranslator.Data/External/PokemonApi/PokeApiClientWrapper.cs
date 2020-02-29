using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokeApiNet;
using Poketranslator.Data.Interfaces.External.PokemonApi;
using Poketranslator.Data.Services;

namespace Poketranslator.Data.External.PokemonApi
{
    public class PokeApiClientWrapper : IPokeApiClientWrapper
    {
        public virtual async Task<TNamedApiResource> GetResourceAsync<TNamedApiResource>(string pokemonName)
            where TNamedApiResource : NamedApiResource
        {
            try
            {
                using var client = new PokeApiClient();

                return await client.GetResourceAsync<TNamedApiResource>(pokemonName).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
    }
}