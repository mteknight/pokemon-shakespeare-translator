using System.Threading.Tasks;

namespace Poketranslator.Data.Interfaces.External.PokemonApi
{
    public interface IPokemonApiService
    {
        Task<Domain.Pokemon> GetByName(string pokemonName);
    }
}