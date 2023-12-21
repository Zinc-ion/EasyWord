using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using EasyWord.Library.Models;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using Moq;
using Xunit;

namespace EasyWord.Test.Services
{
    public class YiYanServiceTest
    {
        [Fact]
        public async Task GetHitikotoAsync_SuccessfulRequest_ShouldReturnHitokoto()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"id\": 1, \"hitokoto\": \"TestHitokoto\", \"type\": \"a\", \"from\": \"TestFrom\", \"from_who\": \"TestFromWho\", \"creator\": \"TestCreator\"}")
            };

            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object)
            {
                BaseAddress = new Uri("https://v1.hitokoto.cn/")
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var yiYanService = new YiYanService(alertServiceMock.Object);

            // Act
            var hitokoto = await yiYanService.GetHitikotoAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.Equal("TestHitokoto", hitokoto.Text);
            Assert.Equal("动画", hitokoto.Type); // Type "a" should map to "动画"
            Assert.Equal("TestFrom", hitokoto.From);
            Assert.Equal("TestFromWho", hitokoto.FromWho);
            Assert.Equal("TestCreator", hitokoto.Creator);
        }

        [Fact]
        public async Task GetHitikotoAsync_FailedRequest_ShouldShowAlert()
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
                BaseAddress = new Uri("https://v1.hitokoto.cn/")
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var yiYanService = new YiYanService(alertServiceMock.Object);

            // Act
            var hitokoto = await yiYanService.GetHitikotoAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.Is<string>(message => message.Contains("TestErrorMessage")),
                It.IsAny<string>()),
                Times.Once);

            Assert.NotNull(hitokoto);
            Assert.Null(hitokoto.Text);
            Assert.Null(hitokoto.Type);
            Assert.Null(hitokoto.From);
            Assert.Null(hitokoto.FromWho);
            Assert.Null(hitokoto.Creator);
        }
    }
}
