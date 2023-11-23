using EasyWord.Library.Models;
using EasyWord.Library.Services;
using EasyWord.Library.Services.Impl;

namespace EasyWord.Library.Pages;


using System;
public partial class News
{

    private TodayArticle _article = new TodayArticle();

    public async Task<TodayArticle> GetNewsAsync()
    {
        _article = await _newsService.GetNewsAsync();
        return _article;
    }

}