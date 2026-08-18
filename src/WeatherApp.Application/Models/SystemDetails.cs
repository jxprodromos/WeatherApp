namespace WeatherApp.Application.Models;

public class SystemDetails
{
    public string CountryCode { get; set; } = string.Empty;

    public long? SunriseTimestampUtc { get; set; }

    public long? SunsetTimestampUtc { get; set; }
}
