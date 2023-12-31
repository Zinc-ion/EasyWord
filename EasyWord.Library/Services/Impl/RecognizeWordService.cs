﻿using System.Text.Json;
using TheSalLab.GeneralReturnValues;

namespace EasyWord.Library.Services.Impl;

public class RecognizeWordService : IRecognizeWordService
{
    private readonly IAlertService _alertService;

    private const string Server = "EasyWord服务器";

    public RecognizeWordService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<string> RecognizeWordAsync(byte[] photo)
    {
        using var httpClient = new HttpClient();

        using var formData = new MultipartFormDataContent();
        formData.Add(new ByteArrayContent(photo), "file", "file.jpg");

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.PostAsync(
                    "http://172.22.19.122:8000/home/image2Word", formData);
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