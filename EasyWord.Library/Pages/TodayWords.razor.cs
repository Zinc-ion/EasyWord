using EasyWord.Library.Models;
using EasyWord.Library.Services;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更过结果";
    private string _status = string.Empty;

    public const int pageSize = 300;

    private List<Word> _words = new();


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
        var words = await _wordStorage.GetFromCET4_1Async(pageSize,_words.Count);
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