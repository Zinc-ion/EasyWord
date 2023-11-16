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

    public const int pageSize = 5;

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


    private async Task LoadMoreAsync()
    {
        var words = await _wordStorage.GetFromCET4_1Async(pageSize);
        _words.AddRange(words);
    }
}