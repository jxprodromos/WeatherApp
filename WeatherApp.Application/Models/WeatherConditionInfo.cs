using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class WeatherConditionInfo
{
    [JsonPropertyName("main")]
    public string ConditionGroup { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string ConditionDescription { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string IconCode { get; set; } = string.Empty;
}
