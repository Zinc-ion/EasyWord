using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;

namespace EasyWord.Test.Services;

//存储服务测试  xj实现
public class ParcelBoxServiceTest
{
    
    [Fact]
    public void Put_ShouldReturnTokenAndStoreObject()
    {
        // Arrange
        var parcelBoxService = new ParcelBoxService();
        var testObject = new { Id = 1, Name = "Test" };

        // Act
        var token = parcelBoxService.Put(testObject);

        // Assert
        Assert.NotNull(token);
        Assert.True(parcelBoxService.Get(token) == testObject);
    }

    [Fact]
    public void Get_WithValidTicket_ShouldReturnStoredObject()
    {
        // Arrange
        var parcelBoxService = new ParcelBoxService();
        var testObject = new { Id = 1, Name = "Test" };
        var token = parcelBoxService.Put(testObject);

        // Act
        var result = parcelBoxService.Get(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testObject, result);
    }

    [Fact]
    public void Get_WithInvalidTicket_ShouldReturnNull()
    {
        // Arrange
        var parcelBoxService = new ParcelBoxService();
        var invalidToken = "invalid-token";

        // Act
        var result = parcelBoxService.Get(invalidToken);

        // Assert
        Assert.Null(result);
    }

}