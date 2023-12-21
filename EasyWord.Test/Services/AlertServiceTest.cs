using System.Threading.Tasks;
using BootstrapBlazor.Components;
using EasyWord.Library.Services.Impl;
using Moq;
using Xunit;

namespace EasyWord.Test.Services
{
    public class AlertServiceTest
    {
        [Fact]
        public async Task AlertAsync_ShouldShowSwalService()
        {
            // Arrange
            var swalServiceMock = new Mock<SwalService>();
            var alertService = new AlertService(swalServiceMock.Object);

            // Act
            await alertService.AlertAsync("Test Title", "Test Message", "Test Button");

            // Assert
            swalServiceMock.Verify(s => s.Show(It.IsAny<SwalOption>()), Times.Once);
        }
    }
}
