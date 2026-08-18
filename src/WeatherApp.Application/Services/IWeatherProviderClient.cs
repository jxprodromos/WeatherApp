using ErrorOr;
using WeatherApp.Application.Models;

namespace WeatherApp.Application.Services;

public interface IWeatherProviderClient
{
    Task<ErrorOr<WeatherDto>> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}
