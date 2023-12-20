using EasyWord.Library.Models;
using System.Linq.Expressions;

namespace EasyWord.Library.Services;

public interface IWordStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task SetBookId(string bookId);

    Task<IEnumerable<Word>> GetWordsAsync(
        Expression<Func<Word, bool>> where, int skip, int take);

    //查找单个单词
    Task<Word> GetWordAsync(int wordRank);

    //查找复习单词
    Task<IEnumerable<Word>> GetReviewWordsAsync();

    //已经记住单词，status赋值为-1.再不会出现在背词表中，将现在时间获取，存入数据库
    Task<int> KnowWord(int wordRank);

    //不认识的单词，将其放入复习列表中
    Task<int> UnknownWord(int wordRank);

    //复习单词
    Task<int> ReviewWord(int wordRank);

    
}