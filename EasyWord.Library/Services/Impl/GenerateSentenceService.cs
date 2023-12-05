using System.Text.Json;
using TheSalLab.GeneralReturnValues;

namespace EasyWord.Library.Services.Impl;

public class GenerateSentenceService : IGenerateSentenceService
{
    private readonly IAlertService _alertService;

    private const string Server = "EasyWord服务器";

    public GenerateSentenceService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<string> GenerateSentenceAsync(string headWord)
    {
        using var httpClient = new HttpClient();

        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(headWord), "word");

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.PostAsync(
                    "http://127.0.0.1:8000/home/sentenceComposing", formData);
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

        return result.Result;
    }
}