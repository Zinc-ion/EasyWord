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


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        var wordRank = Convert.ToInt32(WordRank);
        var word = await _wordStorage.GetWordAsync(wordRank);
        _word = word;

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