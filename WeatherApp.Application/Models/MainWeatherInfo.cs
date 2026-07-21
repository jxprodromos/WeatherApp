using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class MainWeatherInfo
{
    [JsonPropertyName("temp")]
    public double TemperatureCelsius { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLikeCelsius { get; set; }

    [JsonPropertyName("pressure")]
    public int PressureHpa { get; set; }

    [JsonPropertyName("humidity")]
    public int HumidityPercent { get; set; }
}
