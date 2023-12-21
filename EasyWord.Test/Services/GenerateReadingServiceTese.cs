using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EasyWord.Library.Services.Impl;
using Moq;
using Xunit;

namespace EasyWord.Test.Services
{
    public class GenerateReadingServiceTest
    {
        [Fact]
        public async Task GenerateReadingAsync_SuccessfulRequest_ShouldReturnResult()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"status\": \"Succeeded\", \"result\": \"TestResult\" }")
            };

            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object)
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var generateReadingService = new GenerateReadingService(alertServiceMock.Object);

            // Act
            var result = await generateReadingService.GenerateReadingAsync("TestWords");

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.Equal("TestResult", result);
        }

        [Fact]
        public async Task GenerateReadingAsync_FailedRequest_ShouldShowAlert()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent("{\"status\": \"Failed\", \"messages\": [\"TestErrorMessage\"] }")
            };

            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object)
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var generateReadingService = new GenerateReadingService(alertServiceMock.Object);

            // Act
            var result = await generateReadingService.GenerateReadingAsync("TestWords");

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.Is<string>(message => message.Contains("TestErrorMessage")),
                It.IsAny<string>()),
                Times.Once);

            Assert.Null(result);
        }
    }
}
