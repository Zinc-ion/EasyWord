using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;
using Moq;

namespace EasyWord.Test.Helpers;

public class WordStorageHelper {
    //删除数据库
    public static void RemoveDatabaseFile() =>
        File.Delete(WordStorage.WordDbPath);

    //初始化数据库
    public static async Task<WordStorage> GetInitializedPoetryStorage() {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p =>
            p.Get(WordStorageConstant.DbVersionKey, -1)).Returns(-1);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new WordStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();
        return poetryStorage;
    }

    public static async void 
}