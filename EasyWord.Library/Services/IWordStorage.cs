namespace EasyWord.Library.Services;

public interface IWordStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();
}