using System.Text.Json.Serialization;

namespace WeatherApp.Infrastructure.Weather;

internal sealed class OpenWeatherResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("main")]
    public MainResponse Main { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherResponse> Weather { get; set; } = [];

    [JsonPropertyName("wind")]
    public WindResponse Wind { get; set; } = new();

    [JsonPropertyName("sys")]
    public SysResponse Sys { get; set; } = new();

    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    internal sealed class MainResponse
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    internal sealed class WeatherResponse
    {
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    internal sealed class WindResponse
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }

    internal sealed class SysResponse
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("sunrise")]
        public long? Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long? Sunset { get; set; }
    }
}
