using Microsoft.Extensions.Logging;
using WeatherApp.Application.Services;

namespace WeatherApp.Application.Jobs;

/// <summary>
/// Recurring job that fetches current weather for a fixed location and logs the result.
/// This job is executed by Hangfire scheduler at regular intervals.
/// </summary>
public sealed class WeatherFetchJob
{
    // Hardcoded to Athens, Greece for now. Location configuration can be added in future enhancements.
    private const double FixedLatitude = 37.9838;
    private const double FixedLongitude = 23.7275;

    private readonly IWeatherProviderClient _weatherProvider;
    private readonly ILogger<WeatherFetchJob> _logger;

    public WeatherFetchJob(IWeatherProviderClient weatherProvider, ILogger<WeatherFetchJob> logger)
    {
        ArgumentNullException.ThrowIfNull(weatherProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _weatherProvider = weatherProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _weatherProvider.GetCurrentWeatherAsync(FixedLatitude, FixedLongitude, cancellationToken);

            if (result.IsError)
            {
                _logger.LogWarning(
                    "Weather fetch job failed for coordinates ({Latitude}, {Longitude}). Error: {ErrorCode} - {ErrorDescription}",
                    FixedLatitude,
                    FixedLongitude,
                    result.Errors[0].Code,
                    result.Errors[0].Description);
                return;
            }

            var weather = result.Value;
            _logger.LogInformation(
                "Weather fetch job succeeded for {LocationName}: Temperature={TemperatureCelsius}°C, Humidity={HumidityPercent}%, Condition={ConditionGroup}, ObservedAt={ObservationTimestampUtc}",
                weather.LocationName,
                weather.MainWeather.TemperatureCelsius,
                weather.MainWeather.HumidityPercent,
                weather.WeatherConditionInfos.FirstOrDefault()?.ConditionGroup ?? "N/A",
                weather.ObservationTimestampUtc);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Weather fetch job was cancelled for coordinates ({Latitude}, {Longitude}).", FixedLatitude, FixedLongitude);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error in weather fetch job for coordinates ({Latitude}, {Longitude}).",
                FixedLatitude,
                FixedLongitude);
        }
    }
}
