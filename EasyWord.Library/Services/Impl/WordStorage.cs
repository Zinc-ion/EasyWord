namespace EasyWord.Library.Services.Impl;

public class WordStorage : IWordStorage
{
    // TestIsInitialized
    public bool IsInitialized =>
        _preferenceStorage.Get(WordStorageConstant.DbVersionKey, 0) ==
        WordStorageConstant.Version;

    private readonly IPreferenceStorage _preferenceStorage;

    public WordStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }
}


public static class WordStorageConstant
{
    public const string DbVersionKey =
        nameof(WordStorageConstant) + "." + nameof(DbVersionKey);
    // nameof(WordStorageConstant) -> "WordStorageConstant"
    // "WordStorageConstant.DbVersionKey"

    public const int Version = 1;
}