using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using EasyWord.Test.Helpers;
using Moq;

namespace EasyWord.Test.Services;

public class WordStorageTest : IDisposable
{
    //将清理文件的方法放到构造函数中，在每次测试前执行
    public WordStorageTest() => WordStorageHelper.RemoveDatabaseFile();
    //将清理文件的方法放到析构函数中，在每次测试后执行
    public void Dispose() => WordStorageHelper.RemoveDatabaseFile();

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

    [Fact]
    public async Task InitializeAsync_Default()
    {
        //使用Mock来new出键值对存储服务
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var wordStorage = new WordStorage(mockPreferenceStorage);

        //假设文件不存在
        Assert.False(File.Exists(WordStorage.WordDbPath));
        //初始化数据库
        await wordStorage.InitializeAsync();
        //断言文件存在
        Assert.True(File.Exists(WordStorage.WordDbPath));

        preferenceStorageMock.Verify(
            p => p.Set(WordStorageConstant.DbVersionKey,
                WordStorageConstant.Version), Times.Once);
    }

}