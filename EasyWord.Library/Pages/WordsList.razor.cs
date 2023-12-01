using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace EasyWord.Library.Pages;

public partial class WordsList
{
    [Parameter]
    public string BookId { get; set; }

    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更过结果";
    private string _status = string.Empty;

    public const int pageSize = 10;

    private List<Word> _words = new();

    private Expression<Func<Word, bool>> where;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await _wordStorage.InitializeAsync();
        await LoadMoreAsync();
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

    //无限滚动
    public async Task LoadMoreAsync()
    {
        _status = Loading;
        where = p => p.BookId == BookId;
        var words = await _wordStorage.GetWordsAsync(where,pageSize, _words.Count);
        _status = string.Empty;
        _words.AddRange(words);

        if (words.Count() < pageSize)
        {
            _status = NoMoreResult;
        }

        if (!words.Any() && _words.Count == 0)
        {
            _status = NoResult;
        }
    }
}