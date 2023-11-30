namespace EasyWord.Library.Services;

public interface ITTSService
{
    void ToSpeechAsync(string text);
}