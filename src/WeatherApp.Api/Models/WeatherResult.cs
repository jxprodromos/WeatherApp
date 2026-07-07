using System.Text.Json.Serialization;

namespace WeatherApp.Api.Models;

public class WeatherResult
{
    public string Name { get; set; }
    public MainInfo Main { get; set; }
    public List<WeatherInfo> Weather { get; set; }
    public WindInfo Wind { get; set; }
    public SysInfo Sys { get; set; }
    public long Dt { get; set; }
    public class MainInfo
    {
        public double Temp { get; set; }
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
    public class WeatherInfo
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
    public class WindInfo
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
    }
    public class SysInfo
    {
        public string Country { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

}
