using EasyWord.Library.Models;

namespace EasyWord.Library.Services;

public interface INewsService
{
    Task<TodayArticle> GetNewsAsync();
}