using BootstrapBlazor.Components;
using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    private List<Word> _words = new();

    private int pageSize = 5;

    private int totalWords = 0;

    private Expression<Func<Word, bool>> _where = p => p.Status == 0;

    private bool soLittleWords = false;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
    }


    private async void IncrementPageSize()
    {
        ++pageSize;
        if (pageSize > 20)
        {
            soLittleWords = true;
            --pageSize;
            await ToastService.Error("不能再加了", "背这么多要复习不完喽", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }
    private async void DecrementPageSize()
    {
        --pageSize;
        if (pageSize < 1)
        {
            soLittleWords = true;
            ++pageSize;
            await ToastService.Error("不能再减了", "至少要背一个吧", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }

    //认识
    public async Task<int> KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);
        --pageSize;
        ++totalWords;
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }

    //不认识单词
    public async Task<int> UnknownWord(int wordRank)
    {
        await _wordStorage.UnknownWord(wordRank);
        --pageSize;
        ++totalWords;
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }

    private async void GenerateReading()
    {
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.Reading}");
    }


    //单词发音
    private async Task ToSpeech(string word)
    {
        if (!string.IsNullOrEmpty(word))
        {
           
            if (await _tTSService.ToSpeechAsync(word))
            {
                await Task.Delay(2500);
            
            }
        }
    }

    private void OnClick(Word word) =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.WordDetail}/{word.WordRank}");
}