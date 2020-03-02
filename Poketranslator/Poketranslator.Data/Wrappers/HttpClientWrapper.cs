using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Poketranslator.Data.Interfaces.Wrappers;

namespace Poketranslator.Data.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientWrapper(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public virtual async Task<HttpResponseMessage> PostAsync(
            string requestUri,
            HttpContent content,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}