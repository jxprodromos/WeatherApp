namespace WeatherApp.Application.Models;

public class MainWeatherInfo
{
    public double TemperatureCelsius { get; set; }

    public double FeelsLikeCelsius { get; set; }

    public int PressureHpa { get; set; }

    public int HumidityPercent { get; set; }
}
