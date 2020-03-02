using System.Threading;
using System.Threading.Tasks;

namespace Poketranslator.Data.Interfaces.Services
{
    public interface IShakespeareTranslationService
    {
        Task<string> GetTranslation(
            string textToTranslate,
            CancellationToken cancellationToken);
    }
}