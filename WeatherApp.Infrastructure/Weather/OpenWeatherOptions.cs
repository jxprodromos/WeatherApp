using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Infrastructure.Weather;

public sealed class OpenWeatherOptions
{
    public const string SectionName = "OpenWeatherMap";

    [Required]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
