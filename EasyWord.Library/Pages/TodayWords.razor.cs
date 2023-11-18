using EasyWord.Library.Models;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更过结果";
    private string _status = string.Empty;

    public const int pageSize = 20;

    private List<Word> _words = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        // TODO 测试代码
        await _wordStorage.InitializeAsync();

        await LoadMoreAsync();
    }

    //认识
    private async void KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);
        StateHasChanged();
    }

    //不认识
    private async void UnknownWord(int wordRank)
    {
        await _wordStorage.UnknownWord(wordRank);
        StateHasChanged();
    }

    //无限滚动
    private async Task LoadMoreAsync()
    {
        var words = await _wordStorage.GetFromCET4_1Async(pageSize,_words.Count);
        _words.AddRange(words);
    }
}