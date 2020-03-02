using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Services;
using Poketranslator.Data.Tests.Helpers;
using Xunit;

namespace Poketranslator.Data.Tests.Services
{
    public class ShakespeareTranslationServiceTests
    {
        [Theory]
        [InlineData(default(string))]
        [InlineData("")]
        [InlineData(" ")]
        public Task TestGetTranslation_WithInvalidParameters_ExpectArgumentNullException(string textToTranslate)
        {
            // Arrange
            var sutService = DIHelper.GetServices()
                .GetConfiguredService<IShakespeareTranslationService>();

            // Act
            Func<Task> SutCall() => () => sutService.GetTranslation(textToTranslate, CancellationToken.None);

            // Assert
            return Assert.ThrowsAsync<ArgumentNullException>(SutCall());
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGetTranslation_WhenRequestIsMade_ExpectHttpClientWrapperCall(
            Mock<IHttpClientWrapper> httpClientWrapper,
            Dictionary<string, string> parametersDictionary)
        {
            // Arrange
            var textToTranslate = parametersDictionary.Values.FirstOrDefault();
            var httpResponseMessage = CreateHttpResponseMessage();
            SetupHttpClientWrapperMock(httpClientWrapper, httpResponseMessage);

            var sutService = DIHelper.GetServices()
                .RegisterMock(httpClientWrapper)
                .GetConfiguredService<IShakespeareTranslationService>();

            // Act
            await sutService.GetTranslation(textToTranslate, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            httpClientWrapper
                .Verify(wrapper => wrapper.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private static HttpResponseMessage CreateHttpResponseMessage(string content = "")
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content)
            };
        }

        private static void SetupHttpClientWrapperMock(
            Mock<IHttpClientWrapper> httpClientWrapper,
            HttpResponseMessage httpResponseMessage)
        {
            httpClientWrapper
                .Setup(wrapper => wrapper.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => httpResponseMessage);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestGetTranslation_WhenNoDataIsReturned_ExpectNullReturn(
            Mock<IHttpClientWrapper> httpClientWrapper,
            string textToTranslate)
        {
            // Arrange
            var httpResponseMessage = CreateHttpResponseMessage();
            SetupHttpClientWrapperMock(httpClientWrapper, httpResponseMessage);

            var sutService = DIHelper.GetServices()
                .RegisterMock(httpClientWrapper)
                .GetConfiguredService<IShakespeareTranslationService>();

            // Act
            var translation = await sutService.GetTranslation(textToTranslate, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            Assert.Null(translation);
        }

        [Theory]
        [AutoMoqData]
        public async Task TaskGetTranslation_WhenContentHasDuplicatedWhitespaces_ReturnParsedContentWithoutDuplicates(
            Mock<IHttpClientWrapper> httpClientWrapper,
            string textToTranslate)
        {
            // Arrange
            const string translatedText = "Test of  duplicated whitespaces  in here.";
            const string expectedParsedTranslation = "Test of duplicated whitespaces in here.";

            var translationResponse = CreateTranslationResponse(translatedText);
            var json = JsonConvert.SerializeObject(translationResponse);
            var httpResponseMessage = CreateHttpResponseMessage(json);

            SetupHttpClientWrapperMock(httpClientWrapper, httpResponseMessage);

            var sutService = DIHelper.GetServices()
                .RegisterMock(httpClientWrapper)
                .GetConfiguredService<IShakespeareTranslationService>();

            // Act
            var translation = await sutService.GetTranslation(textToTranslate, CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedParsedTranslation, translation);
        }

        private static ShakespeareTranslationResponse CreateTranslationResponse(string translatedText)
        {
            return new ShakespeareTranslationResponse
            {
                Contents = new TranslationContents
                {
                    Translated = translatedText
                }
            };
        }
    }
}
