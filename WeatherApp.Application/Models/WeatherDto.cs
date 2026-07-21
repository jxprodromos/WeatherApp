using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class WeatherDto
{
    [JsonPropertyName("name")]
    public string LocationName { get; set; } = string.Empty;

    [JsonPropertyName("main")]
    public MainWeatherInfo MainWeather { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherConditionInfo> WeatherConditionInfos { get; set; } = [];

    [JsonPropertyName("wind")]
    public WindDetails Wind { get; set; } = new();

    [JsonPropertyName("sys")]
    public SystemDetails System { get; set; } = new();

    [JsonPropertyName("dt")]
    public long ObservationTimestampUtc { get; set; }
}
