using System.Globalization;
using System.Linq.Expressions;
using EasyWord.Library.Models;
using SQLite;
using System;
using Microsoft.VisualBasic;

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

    //存书名
    public async Task SetBookId(string bookId) => 
        _preferenceStorage.Set(WordStorageConstant.BookIdKey, bookId);

    //获取键值存储中的书名
    public string bookId => _preferenceStorage.Get(WordStorageConstant.BookIdKey, "CET_4");

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
        await Connection.Table<Word>().Where(p => p.Status == 0).Skip(index).Take(take).ToListAsync();

    //获得自定义单词
    public async Task<IEnumerable<Word>> GetWordsAsync(Expression<Func<Word, bool>> where, int skip, int take) =>
        await Connection.Table<Word>().Where(where).Skip(skip).Take(take).ToListAsync();


    //单词详情
    public async Task<Word> GetWordAsync(int wordRank)
    {
        Word word = await Connection.Table<Word>()
            .Where(p => p.WordRank == wordRank)
            .FirstOrDefaultAsync();
        return word;
    }




    //关闭数据库连接
    public async Task CloseAsync() => await Connection.CloseAsync();



    //已经记住单词，status赋值为-1.再不会出现在背词表中，将现在时间获取，存入数据库
    public async Task<int> KnowWord(int wordRank)
    {

        Word word = await Connection.Table<Word>()
            .Where(p => p.WordRank == wordRank)
            .FirstOrDefaultAsync();
        Console.WriteLine(word);
        if (word != null)
        {
            //获取时间
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            word.Status = -1;
            word.DateLastReviewed = now;
            word.DateRecite = nowDate;
            await Connection.UpdateAsync(word);
            return 1;
        }
        return 0;
    }


    //不认识的单词，将其放入复习列表中
    public async Task<int> UnknownWord(int wordRank)
    {
        var word = await Connection.Table<Word>()
            .Where(p => p.WordRank == wordRank)
            .FirstOrDefaultAsync();
        if (word != null)
        {
            //获取时间
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //将复习状态变为10，每复习一次就减1减到0就将其变为-1，不再显示
            word.Status = 10;
            word.DateLastReviewed = now;

            // 计算下次复习时间
            DateTime nextReview = DateTime.Now;
            int interval = (10 - word.Status + 1) * 10; // 间隔小时数
            nextReview = nextReview.AddHours(interval); // 增加间隔小时数

            // 转为字符串格式保存
            word.DateNextReview = nextReview.ToString("yyyy-MM-dd HH:mm:ss");
            word.DateRecite = DateTime.Now.ToString("yyyy-MM-dd");

            await Connection.UpdateAsync(word);
            return 1;
        }
        return 0;
    }

    public async Task<int> ReviewWord(int wordRank)
    {
        var word = await Connection.Table<Word>()
            .Where(p => p.WordRank == wordRank)
            .FirstOrDefaultAsync();
        if (word != null)
        {
            //获取时间
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //将复习状态变为10，每复习一次就减1减到0就将其变为-1，不再显示
            if (--word.Status == 0)
            {
                word.Status = -1;
            }
            else
            {
                --word.Status;
                word.DateLastReviewed = now;

                // 计算下次复习时间
                DateTime nextReview = DateTime.Now;
                int interval = (10 - word.Status - 1) * 10; // 间隔小时数
                nextReview = nextReview.AddHours(interval); // 增加间隔小时数

                // 转为字符串格式保存
                word.DateNextReview = nextReview.ToString("yyyy-MM-dd HH:mm:ss");
            }
            await Connection.UpdateAsync(word);
            return 1;
        }
        return 0;
    }

    public async Task<IEnumerable<Word>> GetReviewWordsAsync()
    {
        DateTime now = DateTime.Now;

        Expression<Func<Word, bool>> where;
        var words  = await Connection.Table<Word>().Where(p => true).ToListAsync();

        // 数据处理
        var wordsToReview = new List<Word>();

        foreach (var word in words)
        {
            DateTime dateNextTime;
            if (DateTime.TryParse(word.DateNextReview, out dateNextTime))
            {
                if (dateNextTime <= now)
                {
                    wordsToReview.Add(word);
                }
            }
        }

        return wordsToReview;
    }


}

//数据库相关常量，防止直接拼写打错字 xj实现
public static class WordStorageConstant
{
    public const string DbVersionKey =
        nameof(WordStorageConstant) + "." + nameof(DbVersionKey);
    // nameof(WordStorageConstant) -> "WordStorageConstant"
    // "WordStorageConstant.DbVersionKey"

    public const string BookIdKey =
        nameof(WordStorageConstant) + "." + nameof(BookIdKey);
    public const int Version = 1;
}