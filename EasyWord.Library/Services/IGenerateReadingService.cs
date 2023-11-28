namespace EasyWord.Library.Services;

public interface IGenerateReadingService
{
    Task<string> GenerateReadingAsync(string words);
}