using EasyWord.Library.Models;
using System.Linq.Expressions;

namespace EasyWord.Library.Services;

public interface IWordStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    //CET4_1查找take个单词
    Task<IEnumerable<Word>> GetFromCET4_1Async(int take,int index);

    Task<IEnumerable<Word>> GetWordsAsync(
        Expression<Func<Word, bool>> where, int skip, int take);

    //查找单个单词
    Task<Word> GetWordAsync(int wordRank);

    //认识单词
    Task<int> KnowWord(int wordRank);

    Task<int> UnknownWord(int wordRank);
}