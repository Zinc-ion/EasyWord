namespace EasyWord.Library.Services;

public interface ITTSService
{
    Task<bool> ToSpeechAsync(string text);
}