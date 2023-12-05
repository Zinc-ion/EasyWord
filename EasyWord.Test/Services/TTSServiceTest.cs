using EasyWord.Library.Services.Impl;
using Edge_tts_sharp;
using Moq;

namespace EasyWord.Test.Services;

public class TTSServiceTest
{
    [Fact]
    public async Task ToSpeechAsync_ExecutesWithoutError()
    {
        // 安排
        var ttsService = new TTSService();
        var textToSpeech = "你好，这是一个测试。";
        // 行动
        bool flag = await ttsService.ToSpeechAsync(textToSpeech);
        // 断言和验证
        Assert.True(flag);
    }

}