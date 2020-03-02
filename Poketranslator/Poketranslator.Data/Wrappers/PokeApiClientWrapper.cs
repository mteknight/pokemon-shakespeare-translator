using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PokeApiNet;
using Poketranslator.Data.Interfaces.Wrappers;

namespace Poketranslator.Data.Wrappers
{
    public class PokeApiClientWrapper : IPokeApiClientWrapper
    {
        public virtual async Task<TNamedApiResource> GetResourceAsync<TNamedApiResource>(
            string pokemonName,
            CancellationToken cancellationToken)
            where TNamedApiResource : NamedApiResource
        {
            try
            {
                using var client = new PokeApiClient();

                return await client.GetResourceAsync<TNamedApiResource>(pokemonName, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
    }
}