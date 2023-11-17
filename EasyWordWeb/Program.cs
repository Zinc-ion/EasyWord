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
// ��ӱ��д���
builder.Services.AddBootstrapBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IWordStorage, WordStorage>();
builder.Services.AddScoped<IPreferenceStorage, PreferenceStorage>();

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
