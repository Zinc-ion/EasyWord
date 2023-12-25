namespace EasyWord.Library.Services;

public interface IGenerateImageService
{
    Task<string> GenerateImageAsync(string headWord);
}