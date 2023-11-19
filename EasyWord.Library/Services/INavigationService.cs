namespace EasyWord.Library.Services;

public interface INavigationService {
    void NavigateTo(string uri);

    // void NavigateTo(string uri, object parameter);
}

//导航页面常量
public static class NavigationServiceConstants {
    public const string BooksPage = "/books";
    public const string SentenceGenerationPage = "/sentenceGeneration";
    public const string TodayWordsPage = "/todayWords";
    public const string InitializationPage = "/initialization";
}