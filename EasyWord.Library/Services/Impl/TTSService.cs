using Edge_tts_sharp;
using System.Linq;

namespace EasyWord.Library.Services.Impl;

public class TTSService : ITTSService
{
    public async Task<bool> ToSpeechAsync(string text)
    {
        bool success = false;
        
        Random random = new Random();
        //随机选择一个发音人
        var list = Edge_tts.GetVoice();
        var enList = list.Where(x => x.Name.Contains("en-US")).ToList();

        var voice = enList[random.Next(0, enList.Count - 1)];
        try
        {
            await Task.Run(() =>
            {
                Edge_tts.PlayText(text, voice, -15);
            });
            success = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return success;
       
    }
}