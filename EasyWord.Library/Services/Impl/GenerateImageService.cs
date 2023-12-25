using System.Text.Json;
using Newtonsoft.Json;
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

    public async Task<string> GenerateImageAsync(string headWord)
    {
        using var httpClient = new HttpClient();

        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(headWord), "word");

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.PostAsync(
                    "http://localhost:8000/home/text2Image?command=" + headWord.Split("\n")[1], null);
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
        var result = JsonConvert.DeserializeObject<Rootobject>(json);
        // var result = JsonSerializer.Deserialize<ServiceResultViewModel<string>>(
        //     json,
        //     new JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true,
        //         IncludeFields = true
        //     });
        //
        // if (result.Status != ServiceResultStatus.Succeeded)
        // {
        //     await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
        //         ErrorMessages.GetHttpClientError(Server,
        //             string.Join(" ", result.Messages)),
        //         ErrorMessages.HttpClientErrorButton);
        //     return null;
        // }
        //
        // Console.WriteLine(result.Result);
        //将内层内容转化为json
        // JObject jo = JObject.Parse(result.Result.Trim());
        // //提取出base64图像信息
        // string base64 = jo["data"][1].ToString();
        string base64 = result.data[0].b64_image;

        // byte[] imageBytes = Convert.FromBase64String(base64);

        return base64;
    }

    
}



public class Rootobject
{
    public string id { get; set; }
    public string _object { get; set; }
    public int created { get; set; }
    public Datum[] data { get; set; }
    public Usage usage { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class Datum
{
    public string _object { get; set; }
    public string b64_image { get; set; }
    public int index { get; set; }
}
