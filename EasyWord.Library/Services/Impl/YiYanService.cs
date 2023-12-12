using EasyWord.Library.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace EasyWord.Library.Services.Impl;

public class YiYanService : IYiYanService
{
    private readonly IAlertService _alertService;

    private const string Server = "一言服务器";

    Dictionary<string, string> typeMap = new Dictionary<string, string>() {
        {"a", "动画"},
        {"b", "漫画"},
        {"c", "游戏"},
        {"d", "文学"},
        {"e", "原创"},
        {"f", "来自网络"},
        {"g", "其他"},
        {"h", "影视"},
        {"i", "诗词"},
        {"j", "网易云"},
        {"k", "哲学"},
        {"l", "抖机灵"}
    };

    //构造函数
    public YiYanService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<Hitokoto> GetHitikotoAsync()
    {
        using var httpClient = new HttpClient();
        HttpResponseMessage response;

        try
        {
            response =
                await httpClient.GetAsync("https://v1.hitokoto.cn/");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return new Hitokoto();
        }

        var json = await response.Content.ReadAsStringAsync();
        YiYan yiYan;
        try
        {
            yiYan = JsonSerializer.Deserialize<YiYan>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(
                ErrorMessages.JsonDeserializationErrorTitle,
                ErrorMessages.GetJsonDeserializationError(Server, e.Message),
                ErrorMessages.JsonDeserializationErrorButton);
            return new Hitokoto();
        }

        string type = yiYan.Type;
        if (typeMap.ContainsKey(type))
        {
            type = typeMap[type];
        }
        else
        {
            type = "未知类型";
        }

        return new Hitokoto()
        {
            Text = yiYan.Hitokoto,
            Type = type,
            From = yiYan.From,
            FromWho = yiYan.From_who,
            Creator = yiYan.Creator,
        };
    }
}


public class YiYan
{
    public int Id { get; set; }
    public string Uuid { get; set; }
    public string Hitokoto { get; set; }
    public string Type { get; set; }
    public string From { get; set; }
    public string From_who { get; set; }
    public string Creator { get; set; }
    public int Creator_uid { get; set; }
    public int Reviewer { get; set; }
    public string Commit_from { get; set; }
    public string Created_at { get; set; }
    public int Length { get; set; }
}

