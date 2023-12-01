using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;

namespace EasyWord.Library.Pages;

public partial class Reading
{
    [Parameter]
    public string WordsToReading { get; set; }



    private bool _isReadingAvailable = false;

    private bool _isLoadingReading = false;

    private string _reading = "";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        
        StateHasChanged();
    }


    private async void GenerateReading(string words)
    {
        _isReadingAvailable = true;
        _isLoadingReading = true;
        StateHasChanged();
        //TODO 调用_GenerateSentenceService服务，生成例句
        _reading = await _generateReadingService.GenerateReadingAsync(words);
        _isLoadingReading = false;
        StateHasChanged();
    }
}