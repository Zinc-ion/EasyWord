using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Microsoft.VisualBasic.CompilerServices;

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

}