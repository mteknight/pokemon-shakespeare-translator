using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Interfaces.Wrappers;
using Poketranslator.Data.Services.Models;

namespace Poketranslator.Data.Services
{
    public class ShakespeareTranslationService : IShakespeareTranslationService
    {
        private const string Api = @"https://api.funtranslations.com/translate/shakespeare";

        private readonly IHttpClientWrapper _httpClientWrapper;

        public ShakespeareTranslationService(
            IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
        }

        public async Task<string> GetTranslation(
            string textToTranslate,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(textToTranslate))
            {
                throw new ArgumentNullException("Value cannot be null or whitespace.", nameof(textToTranslate));
            }

            var payload = CreatePostPayload(textToTranslate);
            var response = await _httpClientWrapper.PostAsync(Api, payload, cancellationToken).ConfigureAwait(false);
            var translationResponse = await ReadTranslationResponse(response).ConfigureAwait(false);

            return translationResponse?.Contents is null
                ? default
                : ParseDuplicatedWhitespaces(translationResponse);
        }

        private static FormUrlEncodedContent CreatePostPayload(string textToTranslate)
        {
            var parametersDictionary = new Dictionary<string, string>
            {
                {"text", textToTranslate}
            };

            return new FormUrlEncodedContent(parametersDictionary);
        }

        private static async Task<ShakespeareTranslationResponse> ReadTranslationResponse(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ShakespeareTranslationResponse>(responseJson);
        }

        private static string ParseDuplicatedWhitespaces(ShakespeareTranslationResponse translationResponse)
        {
            return Regex.Replace(translationResponse.Contents.Translated, @"[ ]{2,}", " ");
        }
    }
}