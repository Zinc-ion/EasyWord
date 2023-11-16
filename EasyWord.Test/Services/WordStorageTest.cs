using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using Moq;

namespace EasyWord.Test.Services;

public class WordStorageTest
{
    [Fact]
    public void IsInitialized_Initialized()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock
            .Setup(p => p.Get(WordStorageConstant.DbVersionKey, 0))
            .Returns(WordStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var wordStorage = new WordStorage(mockPreferenceStorage);
        Assert.True(wordStorage.IsInitialized);
    }

    [Fact]
    public void IsInitialized_NotInitialized()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock
            .Setup(p => p.Get(WordStorageConstant.DbVersionKey, 0))
            .Returns(0);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var wordStorage = new WordStorage(mockPreferenceStorage);
        Assert.False(wordStorage.IsInitialized);
    }
}