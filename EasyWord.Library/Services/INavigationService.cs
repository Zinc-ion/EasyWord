namespace EasyWord.Library.Services;

//导航Service，用于页面跳转 xj实现.
public interface INavigationService {
    void NavigateTo(string uri);

    void NavigateTo(string uri, object parameter);
}

//导航页面常量.
public static class NavigationServiceConstants {
    public const string BooksPage = "/books";
    public const string TodayWordsPage = "/todayWords";
    public const string InitializationPage = "/initialization";
    public const string DetailPage = "/detail";
    public const string WordDetail = "/wordDetail";
    public const string Reading = "/reading";
}