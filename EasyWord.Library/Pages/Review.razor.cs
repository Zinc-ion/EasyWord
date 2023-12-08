using BootstrapBlazor.Components;
using EasyWord.Library.Models;
using EasyWord.Library.Services;
using System.Linq.Expressions;

namespace EasyWord.Library.Pages;

public partial class Review
{
    private List<Word> _words = new();

    private int pageSize = 5;

    private Expression<Func<Word, bool>> _where = p => p.Status == 0;

    private bool soLittleWords = false;

    private string _wordsToReading = "";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        var words = await _wordStorage.GetReviewWordsAsync();
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
    }

    //认识
    public async Task<int> KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);
        StateHasChanged();
        var words = await _wordStorage.GetReviewWordsAsync();
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }

    //不认识单词
    public async Task<int> ReviewNextTime(int wordRank)
    {
        await _wordStorage.ReviewWord(wordRank);
        StateHasChanged();
        var words = await _wordStorage.GetReviewWordsAsync();
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
        return 1;
    }


    private void OnClick(Word word) =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.WordDetail}/{word.WordRank}");
}