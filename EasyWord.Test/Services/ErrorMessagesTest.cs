using EasyWord.Library.Services.Impl;
using Xunit;

namespace EasyWord.Test.Services
{
    public class ErrorMessagesTest
    {
        [Fact]
        public void GetHttpClientError_ShouldReturnCorrectMessage()
        {
            // Arrange
            string server = "TestServer";
            string message = "TestMessage";

            // Act
            string result = ErrorMessages.GetHttpClientError(server, message);

            // Assert
            Assert.Equal($"与{server}连接时发生了错误：\n{message}", result);
        }

        [Fact]
        public void GetJsonDeserializationError_ShouldReturnCorrectMessage()
        {
            // Arrange
            string server = "TestServer";
            string message = "TestMessage";

            // Act
            string result = ErrorMessages.GetJsonDeserializationError(server, message);

            // Assert
            Assert.Equal($"从{server}读取数据时发生了错误：\n{message}", result);
        }
    }
}
