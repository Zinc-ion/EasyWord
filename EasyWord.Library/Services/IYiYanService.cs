using EasyWord.Library.Models;

namespace EasyWord.Library.Services;

public interface IYiYanService
{
    Task<Hitokoto> GetHitikotoAsync();
}