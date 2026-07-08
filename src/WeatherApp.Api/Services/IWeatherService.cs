using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

public interface IWeatherService
{
    Task<WeatherResult> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}
