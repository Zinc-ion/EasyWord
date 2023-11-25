using Microsoft.AspNetCore.Components;

namespace EasyWord.Library.Pages;

public partial class WordDetail
{
    [Parameter]
    public int WordRank { get; set; }
}