using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class WindDetails
{
    [JsonPropertyName("speed")]
    public double SpeedMetersPerSecond { get; set; }

    [JsonPropertyName("deg")]
    public int DirectionDegrees { get; set; }
}
