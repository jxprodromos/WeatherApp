namespace WeatherApp.Application.Models;

public class WeatherDto
{
    public string LocationName { get; set; } = string.Empty;

    public MainWeatherInfo MainWeather { get; set; } = new();

    public List<WeatherConditionInfo> WeatherConditionInfos { get; set; } = [];

    public WindDetails Wind { get; set; } = new();

    public SystemDetails System { get; set; } = new();

    public long ObservationTimestampUtc { get; set; }
}
