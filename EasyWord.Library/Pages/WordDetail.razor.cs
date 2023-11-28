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

    private bool _isLoadingPoetry = true;

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

        _isLoadingPoetry = true;
        StateHasChanged();

        _word = await _wordStorage.GetWordAsync(wordRank);

        _isLoadingPoetry = false;
        StateHasChanged();
    }


    //认识
    public async Task<int> KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);
        return 1;
    }

    //不认识单词
    public async Task<int> UnknownWord(int wordRank)
    {
        await _wordStorage.UnknownWord(wordRank);
        return 1;
    }

}