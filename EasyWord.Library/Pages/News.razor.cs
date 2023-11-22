using EasyWord.Library.Services;
namespace EasyWord.Library.Pages;

using System;
public partial class News
{
    //private ArticlesResult articles = GetArticlesAsync();

    /**
    public static ArticlesResult GetArticlesAsync()
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
    }**/
}