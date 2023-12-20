using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using EasyWord.Library.Services;

namespace EasyWord.Library.Pages;

public partial class Reading
{
    private bool _isReadingAvailable = false;

    private bool _isLoadingReading = false;

    private string _wordsToReading = "";

    private string _reading = "";

    private List<Word> _words = new();

    private static string nowDate = DateTime.Now.ToString("yyyy-MM-dd");

    private Expression<Func<Word, bool>> _where = p => p.DateRecite == nowDate;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        var words = await _wordStorage.GetWordsAsync(_where, 0,1500);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
    }

    private void addWord(string headWord)
    {
        _wordsToReading = _wordsToReading +" " + headWord;
        StateHasChanged();
    }

    private async void GenerateReading()
    {
        _isLoadingReading = true;
        _isReadingAvailable = true;
        StateHasChanged();
        //TODO 调用_GenerateSentenceService服务，生成例句
        _reading = await _generateReadingService.GenerateReadingAsync(_wordsToReading);
        _isLoadingReading = false;

        StateHasChanged();
    }


    private void OnClick(Word word) =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.WordDetail}/{word.WordRank}");

    private void GoTodayWord() =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.TodayWordsPage}");
}