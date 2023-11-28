using EasyWord.Library.Services.Impl;
using EasyWord.Library.Services;
using EasyWord.Web.Data;
using EasyWord.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


// 添加本行代码
builder.Services.AddBootstrapBlazor();
builder.Services.AddScoped<IWordStorage, WordStorage>();
builder.Services.AddScoped<IPreferenceStorage, PreferenceStorage>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IParcelBoxService, ParcelBoxService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IGenerateSentenceService, GenerateSentenceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
