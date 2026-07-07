using WeatherApp.Api.Extensions;
using WeatherApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/weather", async (double lat, double lon, IWeatherService weatherService) =>
{
    return await weatherService.GetCurrentWeatherAsync(lat, lon);
});

app.Run();