using System.Threading;
using System.Threading.Tasks;
using Poketranslator.Domain.Interfaces.Domain;

namespace Poketranslator.Data.Interfaces.Services
{
    public interface IPokemonApiService
    {
        Task<IPokemon> GetByName(
            string pokemonName,
            CancellationToken cancellationToken);
    }
}