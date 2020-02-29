using System.Threading.Tasks;
using PokeApiNet;

namespace Poketranslator.Data.Interfaces.External.PokemonApi
{
    public interface IPokeApiClientWrapper
    {
        Task<TNamedApiResource> GetResourceAsync<TNamedApiResource>(string pokemonName)
            where TNamedApiResource : NamedApiResource;
    }
}