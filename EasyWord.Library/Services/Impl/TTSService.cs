using Edge_tts_sharp;

namespace EasyWord.Library.Services.Impl;

public class TTSService : ITTSService
{
    private string[] names = { "Microsoft Server Speech Text to Speech Voice (en-US, AriaNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, AnaNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, ChristopherNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, EricNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, GuyNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, JennyNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, MichelleNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, RogerNeural)",
        "Microsoft Server Speech Text to Speech Voice (en-US, SteffanNeural)" };
    public async void ToSpeechAsync(string text)
    {
        await Task.Run(() =>
        {
            Random random = new Random();
            var voice = Edge_tts.GetVoice().FirstOrDefault(i => i.Name == names[random.Next(0, 8)]);
            Edge_tts.PlayText(text, voice, -15);
        });
        //随机选择一个发音人
    }
}