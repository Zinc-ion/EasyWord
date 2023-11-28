namespace EasyWord.Library.Services;

public interface IGenerateSentenceService
{
    Task<string> GenerateSentenceAsync(string headWord);
}