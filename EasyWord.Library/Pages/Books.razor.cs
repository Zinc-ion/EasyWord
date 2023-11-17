using EasyWord.Library.Models;

namespace EasyWord.Library.Pages;

public partial class Books
{
    private List<Book> _books = new ();


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        // TODO 测试代码
        await _wordStorage.InitializeAsync();

        var words = await _wordStorage.GetBooksAsync();
        _books.AddRange(words);
    }


}