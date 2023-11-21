namespace EasyWord.Library.Services;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System;


public interface INewsApi
{   
    ArticlesResult GetArticlesAsync(int id);
}