namespace EasyWord.Library.Services;

public interface IGenerateImageService
{
    Task<byte[]> GenerateImageAsync(string headWord);
}