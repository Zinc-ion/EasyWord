using EasyWord.Library.Models;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using EasyWord.Test.Helpers;
using Moq;
using System.Linq.Expressions;

namespace EasyWord.Test.Services;

public class WordStorageTest : IDisposable
{
    //将清理文件的方法放到构造函数中，在每次测试前执行 xj实现
    public WordStorageTest() => WordStorageHelper.RemoveDatabaseFile();
    //将清理文件的方法放到析构函数中，在每次测试后执行 xj实现
    public void Dispose() => WordStorageHelper.RemoveDatabaseFile();

    [Fact]
    public void IsInitialized_Initialized()
    {
        // xj实现
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
        // xj实现
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
        //使用Mock来new出键值对存储服务 xj实现
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

    
    [Fact]
    public async Task KnowWord_default()
    {
        var wordStorage = await WordStorageHelper.GetInitializedPoetryStorage();
        var result = await wordStorage.KnowWord(2);
        Assert.Equal(1,result);
        await wordStorage.CloseAsync();
    }

    [Fact]
    public async Task UnknownWord_default()
    {
        var wordStorage = await WordStorageHelper.GetInitializedPoetryStorage();
        var result = await wordStorage.UnknownWord(2);
        Assert.Equal(1, result);
        await wordStorage.CloseAsync();
    }



    // GetWordsAsync(
        //Expression<Func<Word, bool>> where, int skip, int take);

    //查找单个单词
   // Task<Word> GetWordAsync(int wordRank);



    [Fact]
    public async Task ReviewWord()
    {
        var wordStorage = await WordStorageHelper.GetInitializedPoetryStorage();
        var result = await wordStorage.ReviewWord(5);
        Assert.Equal(1, result);
        await wordStorage.CloseAsync();
    }

    [Fact]
    public async Task GetReviewWordsAsync_Default()
    {
        var wordStorage =
            await WordStorageHelper.GetInitializedPoetryStorage();
        var words = await wordStorage.GetReviewWordsAsync();
        Assert.NotEmpty(words);
        await wordStorage.CloseAsync();
    }

}