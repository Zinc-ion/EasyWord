using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using Moq;
using Xunit;

namespace EasyWord.Test.Services
{
    public class YiYanServiceTest
    {
        [Fact(Skip = "依赖远程服务的测试")]
        public async Task GetHitokotoAsync_ReturnFromYiyan()
        {
            var alertServiceMock = new Mock<IAlertService>();
            var mockAlertService = alertServiceMock.Object;

            var yiYanService = new YiYanService(mockAlertService);
            var hitokoto = await yiYanService.GetHitikotoAsync();

            Assert.Equal("YiYan", hitokoto.Source);
            alertServiceMock.Verify(
                p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()), Times.Never);
        }
    }
}
