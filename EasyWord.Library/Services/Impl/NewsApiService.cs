using NewsAPI.Constants;
using NewsAPI;
using NewsAPI.Models;

namespace EasyWord.Library.Services.Impl;

public class NewsApiService : INewsApi
{
    public ArticlesResult GetArticlesAsync(int id)
    {
        // init with your API key
        var newsApiClient = new NewsApiClient("72e2a175fb9d4e4b87b21186be600d0d");
        var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
        {
            Q = "Apple",
            SortBy = SortBys.Popularity,
            Language = Languages.EN,
            From = new DateTime(2018, 1, 25),
            PageSize = 1,
            Page = 1
        });
        return articlesResponse.Status == Statuses.Ok ? articlesResponse : new ArticlesResult();
    }
}