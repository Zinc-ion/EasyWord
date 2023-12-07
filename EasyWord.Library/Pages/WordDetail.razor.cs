using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Microsoft.VisualBasic.CompilerServices;
using BootstrapBlazor.Components;

namespace EasyWord.Library.Pages;

public partial class WordDetail
{
    [Parameter]
    public string WordRank { get; set; }

    private Word _word = new();

    private bool _isLoadingWord = true;

    private bool _isSentenceAvailable = false;

    private bool _isLoadingSentence = false;

    private string _sentence = "";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(WordRank))
        {
            return;
        }

        if (!int.TryParse(WordRank, out var wordRank))
        {
            return;
        }

        _isLoadingWord = true;
        StateHasChanged();

        _word = await _wordStorage.GetWordAsync(wordRank);

        _isLoadingWord = false;
        StateHasChanged();
    }


    private async void GenerateSentence(string headWord)
    {
        _isSentenceAvailable = true;
        _isLoadingSentence = true;
        StateHasChanged();
        //TODO 调用_GenerateSentenceService服务，生成例句
        _sentence = await _generateSentenceService.GenerateSentenceAsync(headWord);
        _isLoadingSentence = false;
        StateHasChanged();
    }


    //单词发音
    private bool Start { get; set; }

    private async Task ToSpeech(string word)
    {
        if (!string.IsNullOrEmpty(word))
        {
            SpeechOn();
            if (await _tTSService.ToSpeechAsync(word))
            {
                await Task.Delay(2500);
                SpeechOff();
            }
        }
    }

    private void SpeechOn()
    {
        Start = true;
        StateHasChanged();
    }

    private void SpeechOff()
    {
        Start = false;
        StateHasChanged();
    }

    //例句发音
    private bool Start2 { get; set; }

    private async Task SentenceToSpeech(string sentence)
    {
        var enSentence = sentence.Split("/n")[0];
        if (!string.IsNullOrEmpty(enSentence))
        {
            SentenceSpeechOn();
            if (await _tTSService.ToSpeechAsync(enSentence))
            {
                await Task.Delay(8000);
                SentenceSpeechOff();
            }
        }
    }

    private void SentenceSpeechOn()
    {
        Start2 = true;
        StateHasChanged();
    }

    private void SentenceSpeechOff()
    {
        Start2 = false;
        StateHasChanged();
    }




}