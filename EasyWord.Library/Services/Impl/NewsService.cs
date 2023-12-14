using EasyWord.Library.Models;
using System.Text.Json;

namespace EasyWord.Library.Services.Impl;

public class NewsService : INewsService
{
    private readonly IAlertService _alertService;

    private const string Server = "NewsApi服务器";


    //构造函数
    public NewsService(IAlertService alertService)
    {
        _alertService = alertService;
    }


    public async Task<TodayArticle> GetNewsAsync()
    {
        using var httpClient = new HttpClient();
        //httpClient.DefaultRequestHeaders.Add("apiKey", "72e2a175fb9d4e4b87b21186be600d0d");

        HttpResponseMessage response;

        try
        {
            response =
                await httpClient.GetAsync("https://newsapi.org/v2/everything?q=Apple&sortBy=popularity&apiKey=72e2a175fb9d4e4b87b21186be600d0d");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return new TodayArticle();
        }

        var json = await response.Content.ReadAsStringAsync();

        NewsMessage newsMessage;

        try
        {
            newsMessage = JsonSerializer.Deserialize<NewsMessage>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(
                ErrorMessages.JsonDeserializationErrorTitle,
                ErrorMessages.GetJsonDeserializationError(Server, e.Message),
                ErrorMessages.JsonDeserializationErrorButton);
            return new TodayArticle();
        }

        return new TodayArticle()
        {
            Title = newsMessage.Articles[1].Title,
            Author = newsMessage.Articles[1].Author,
            Name = newsMessage.Articles[1].Source.Name,
            Description = newsMessage.Articles[1].Description,
            Url = newsMessage.Articles[1].Url,
            UrlToImage = newsMessage.Articles[1].UrlToImage,
            PublishedAt = newsMessage.Articles[1].PublishedAt,
            Content = newsMessage.Articles[1].Content,
        };
    }
}


public class NewsMessage
{
    public string Status { get; set; }
    public int TotalResults { get; set; }
    public Article[] Articles { get; set; }
}

public class Article
{
    public Source Source { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string UrlToImage { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Content { get; set; }
}

public class Source
{
    public string Id { get; set; }
    public string Name { get; set; }
}
