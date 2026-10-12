using WeatherApp.Application.Models;

namespace WeatherApp.Infrastructure.Weather;

internal static class OpenWeatherMapper
{
    public static WeatherDto ToDto(OpenWeatherResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        OpenWeatherResponse.WeatherResponse? firstCondition = response.Weather.FirstOrDefault();

        DateTimeOffset observedAt = FromUnixSeconds(response.Dt);
        DateTimeOffset? sunrise = response.Sys.Sunrise.HasValue
            ? FromUnixSeconds(response.Sys.Sunrise.Value)
            : null;
        DateTimeOffset? sunset = response.Sys.Sunset.HasValue
            ? FromUnixSeconds(response.Sys.Sunset.Value)
            : null;

        return new WeatherDto
        {
            LocationName = response.Name,
            MainWeather = new MainWeatherInfo
            {
                TemperatureCelsius = response.Main.Temp,
                FeelsLikeCelsius = response.Main.FeelsLike,
                PressureHpa = response.Main.Pressure,
                HumidityPercent = response.Main.Humidity,
            },
            WeatherConditionInfos =
            [
                new WeatherConditionInfo
                {
                    ConditionGroup = firstCondition?.Main ?? string.Empty,
                    ConditionDescription = firstCondition?.Description ?? string.Empty,
                    IconCode = firstCondition?.Icon ?? string.Empty,
                },
            ],
            Wind = new WindDetails
            {
                SpeedMetersPerSecond = response.Wind.Speed,
                DirectionDegrees = response.Wind.Deg,
            },
            System = new SystemDetails
            {
                CountryCode = response.Sys.Country,
                SunriseTimestampUtc = sunrise?.ToUnixTimeSeconds(),
                SunsetTimestampUtc = sunset?.ToUnixTimeSeconds(),
            },
            ObservationTimestampUtc = observedAt.ToUnixTimeSeconds(),
        };
    }

    private static DateTimeOffset FromUnixSeconds(long unixSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
    }
}
