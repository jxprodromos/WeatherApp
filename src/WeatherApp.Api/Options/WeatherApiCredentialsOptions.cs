using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Options;

public class WeatherApiCredentialsOptions
{
    public const string SectionName = "OpenWeatherMap";

    [Required]
    public string Url { get; set; }

    [Required]
    public string ApiKey { get; set; }

}
