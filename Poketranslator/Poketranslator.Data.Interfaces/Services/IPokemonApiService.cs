using System.Threading;
using System.Threading.Tasks;

namespace Poketranslator.Data.Interfaces.Services
{
    public interface IPokemonApiService
    {
        Task<Domain.Pokemon> GetByName(
            string pokemonName,
            CancellationToken cancellationToken);
    }
}