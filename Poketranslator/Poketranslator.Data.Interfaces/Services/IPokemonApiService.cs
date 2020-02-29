using System.Threading.Tasks;

namespace Poketranslator.Data.Interfaces.Services
{
    public interface IPokemonApiService
    {
        Task<Domain.Pokemon> GetByName(string pokemonName);
    }
}