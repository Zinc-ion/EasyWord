namespace EasyWord.Library.Services;

public interface IRecognizeWordService
{
    Task<string> RecognizeWordAsync(byte[] photo);
}