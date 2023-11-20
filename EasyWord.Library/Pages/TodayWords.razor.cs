using EasyWord.Library.Models;
using System.Linq.Expressions;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    private List<Word> _words = new();

    private int pageSize = 5;

    private Expression<Func<Word, bool>> _where = p => p.status == 0;

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
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }
    private async void DecrementPageSize()
    {
        --pageSize;
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
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