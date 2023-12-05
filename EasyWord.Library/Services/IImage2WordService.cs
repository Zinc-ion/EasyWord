namespace EasyWord.Library.Services;

public interface IImage2WordService
{
    Task<string> ToWordAsync(byte[] photo);
}