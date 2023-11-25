using System.Linq.Expressions;
using EasyWord.Library.Models;
using SQLite;

namespace EasyWord.Library.Services.Impl;

public class WordStorage : IWordStorage
{
    //数据库名 xj实现
    public const string DbName = "inami.db";
    //数据库路径  xj实现
    public static readonly string WordDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbName);
    //建立数据库连接 xj实现
    private SQLiteAsyncConnection _connection;
    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(WordDbPath);

    //键值对存储 xj实现
    private readonly IPreferenceStorage _preferenceStorage;
    //依赖注入键值对存储接口 xj实现
    public WordStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    // TestIsInitialized xj实现
    public bool IsInitialized =>
        _preferenceStorage.Get(WordStorageConstant.DbVersionKey, 0) ==
        WordStorageConstant.Version;


    //异步初始化数据库 xj实现
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
    public async Task<IEnumerable<Word>> GetFromCET4_1Async(int take,int index) =>
        await Connection.Table<Word>().Where(p => p.status == 0).Skip(index).Take(take).ToListAsync();

    public async Task<IEnumerable<Word>> GetWordsAsync(Expression<Func<Word, bool>> where, int skip, int take) =>
        await Connection.Table<Word>().Where(where).Skip(skip).Take(take).ToListAsync();


    //关闭数据库连接
    public async Task CloseAsync() => await Connection.CloseAsync();


    public async Task<IEnumerable<Book>> GetBooksAsync() =>
        await Connection.Table<Book>().ToListAsync();


    public async Task<int> KnowWord(int wordRank)
    {
        Word word = await Connection.Table<Word>()
            .Where(p => p.wordRank == wordRank)
            .FirstOrDefaultAsync();
        Console.WriteLine(word);
        if (word != null)
        {
            word.status = 1;
            await Connection.UpdateAsync(word);
            return 1;
        }
        return 0;
    }

    public async Task<int> UnknownWord(int wordRank)
    {
        var word = await Connection.Table<Word>()
            .Where(p => p.wordRank == wordRank)
            .FirstOrDefaultAsync();
        if (word != null)
        {
            word.status = -1;
            await Connection.UpdateAsync(word);
            return 1;
        }
        return 0;
    }
}

//数据库相关常量，防止直接拼写打错字 xj实现
public static class WordStorageConstant
{
    public const string DbVersionKey =
        nameof(WordStorageConstant) + "." + nameof(DbVersionKey);
    // nameof(WordStorageConstant) -> "WordStorageConstant"
    // "WordStorageConstant.DbVersionKey"

    public const int Version = 1;
}