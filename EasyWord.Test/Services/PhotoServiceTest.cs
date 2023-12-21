using System;
using System.IO;
using System.Threading.Tasks;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using Moq;
using Xamarin.Essentials;
using Xunit;

namespace EasyWord.Test.Services
{
    public class PhotoServiceTest
    {
        [Fact]
        public async Task CaptureAsync_DeviceNotSupported_ShouldShowAlertAndReturnNull()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var photoService = new PhotoService(alertServiceMock.Object);

            var mediaPickerMock = new Mock<IMediaPicker>();
            mediaPickerMock.Setup(picker => picker.IsCaptureSupported).Returns(false);
            MediaPicker.Default = mediaPickerMock.Object;

            // Act
            var result = await photoService.CaptureAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.Is<string>(message => message.Contains("设备不支持拍照")),
                It.IsAny<string>()),
                Times.Once);

            Assert.Null(result);
        }

        [Fact]
        public async Task CaptureAsync_SuccessfulCapture_ShouldReturnByteArray()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var photoService = new PhotoService(alertServiceMock.Object);

            var mediaPickerMock = new Mock<IMediaPicker>();
            mediaPickerMock.Setup(picker => picker.IsCaptureSupported).Returns(true);
            mediaPickerMock.Setup(picker => picker.CapturePhotoAsync()).ReturnsAsync(new FileResult("path/to/photo.jpg"));
            MediaPicker.Default = mediaPickerMock.Object;

            // Act
            var result = await photoService.CaptureAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task PickAsync_SuccessfulPick_ShouldReturnByteArray()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var photoService = new PhotoService(alertServiceMock.Object);

            var mediaPickerMock = new Mock<IMediaPicker>();
            mediaPickerMock.Setup(picker => picker.PickPhotoAsync()).ReturnsAsync(new FileResult("path/to/photo.jpg"));
            MediaPicker.Default = mediaPickerMock.Object;

            // Act
            var result = await photoService.PickAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task PickAsync_NoPhotoSelected_ShouldShowAlertAndReturnNull()
        {
            // Arrange
            var alertServiceMock = new Mock<IAlertService>();
            var photoService = new PhotoService(alertServiceMock.Object);

            var mediaPickerMock = new Mock<IMediaPicker>();
            mediaPickerMock.Setup(picker => picker.PickPhotoAsync()).ReturnsAsync((FileResult)null);
            MediaPicker.Default = mediaPickerMock.Object;

            // Act
            var result = await photoService.PickAsync();

            // Assert
            alertServiceMock.Verify(alert => alert.AlertAsync(
                It.IsAny<string>(),
                It.Is<string>(message => message.Contains("未能选择照片")),
                It.IsAny<string>()),
                Times.Once);

            Assert.Null(result);
        }
    }
}
