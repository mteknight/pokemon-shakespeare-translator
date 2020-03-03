using System.Threading;
using System.Threading.Tasks;
using Poketranslator.Domain.Interfaces.Domain;
using Poketranslator.Domain.Interfaces.Models;

namespace Poketranslator.Domain.Interfaces.Services
{
    public interface IPokemonTranslationService
    {
        Task<IPokemonModel> Translate(
            string pokemonName,
            CancellationToken none);
    }
}