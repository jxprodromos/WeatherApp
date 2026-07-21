using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class SystemDetails
{
    [JsonPropertyName("country")]
    public string CountryCode { get; set; } = string.Empty;

    [JsonPropertyName("sunrise")]
    public long? SunriseTimestampUtc { get; set; }

    [JsonPropertyName("sunset")]
    public long? SunsetTimestampUtc { get; set; }
}
