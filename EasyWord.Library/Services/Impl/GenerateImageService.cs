using System.Text.Json;
using Newtonsoft.Json.Linq;
using TheSalLab.GeneralReturnValues;

namespace EasyWord.Library.Services.Impl;

public class GenerateImageService : IGenerateImageService
{
    private readonly IAlertService _alertService;

    private const string Server = "EasyWord服务器";

    public GenerateImageService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<byte[]> GenerateImageAsync(string headWord)
    {
        using var httpClient = new HttpClient();

        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(headWord), "word");

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.PostAsync(
                    "http://localhost:8000/home/text2Image", formData);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ServiceResultViewModel<string>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            });

        if (result.Status != ServiceResultStatus.Succeeded)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server,
                    string.Join(" ", result.Messages)),
                ErrorMessages.HttpClientErrorButton);
            return null;
        }


        //将内层内容转化为json
        JObject jo = JObject.Parse(result.Result);
        //提取出base64图像信息
        string base64 = jo["data"][1].ToString();

        byte[] imageBytes = Convert.FromBase64String(base64);

        return imageBytes;
    }
}


