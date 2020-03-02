using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Poketranslator.Data.Interfaces.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsync(
            string requestUri,
            HttpContent content,
            CancellationToken cancellationToken);
    }
}