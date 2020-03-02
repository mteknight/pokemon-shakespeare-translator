using System.Threading;
using System.Threading.Tasks;
using Poketranslator.Domain.Interfaces.Domain;

namespace Poketranslator.Domain.Interfaces.Services
{
    public interface IPokemonTranslationService
    {
        Task<IPokemon> Translate(
            string pokemonName,
            CancellationToken none);
    }
}