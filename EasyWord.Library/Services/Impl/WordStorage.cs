using System.Linq.Expressions;
using EasyWord.Library.Models;
using SQLite;

namespace EasyWord.Library.Services.Impl;

public class WordStorage : IWordStorage
{
    //数据库名
    public const string DbName = "inami.db";
    //数据库路径
    public static readonly string WordDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbName);
    //建立数据库连接
    private SQLiteAsyncConnection _connection;
    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(WordDbPath);

    //键值对存储
    private readonly IPreferenceStorage _preferenceStorage;
    //依赖注入键值对存储接口
    public WordStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    // TestIsInitialized
    public bool IsInitialized =>
        _preferenceStorage.Get(WordStorageConstant.DbVersionKey, 0) ==
        WordStorageConstant.Version;




    //异步初始化数据库
    public async Task InitializeAsync()
    {
        //打开文件流
        await using var dbFileStream =
            new FileStream(WordDbPath, FileMode.OpenOrCreate);
        //打开资源流
        await using var dbAssetStream =
            typeof(WordStorage).Assembly.GetManifestResourceStream(DbName);
        //copy流
        await dbAssetStream.CopyToAsync(dbFileStream);

        //存储版本号
        _preferenceStorage.Set(WordStorageConstant.DbVersionKey,
            WordStorageConstant.Version);
    }


    //实现返回CET4_1中的take数量的未背诵单词
    public async Task<IEnumerable<Word>> GetFromCET4_1Async(int take) =>
        await Connection.Table<Word>().Where(p => p.status == 0).Take(take).ToListAsync();

    //关闭数据库连接
    public async Task CloseAsync() => await Connection.CloseAsync(); 
    
}

//数据库相关常量，防止直接拼写打错字
public static class WordStorageConstant
{
    public const string DbVersionKey =
        nameof(WordStorageConstant) + "." + nameof(DbVersionKey);
    // nameof(WordStorageConstant) -> "WordStorageConstant"
    // "WordStorageConstant.DbVersionKey"

    public const int Version = 1;
}