using EasyWord.Library.Models;
using System.Linq.Expressions;

namespace EasyWord.Library.Services;

public interface IWordStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    //查找单词
    Task<IEnumerable<Word>> GetFromCET4_1Async(int take);
}