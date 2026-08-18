using ErrorOr;
using WeatherApp.Application.Models;

namespace WeatherApp.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherProviderClient weatherProviderClient;

    public WeatherService(IWeatherProviderClient weatherProviderClient)
    {
        ArgumentNullException.ThrowIfNull(weatherProviderClient);
        this.weatherProviderClient = weatherProviderClient;
    }

    public Task<ErrorOr<WeatherDto>> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        if (latitude is < -90 or > 90)
        {
            return Task.FromResult<ErrorOr<WeatherDto>>(Error.Validation(
                code: "Weather.InvalidLatitude",
                description: "Latitude must be between -90 and 90."));
        }

        if (longitude is < -180 or > 180)
        {
            return Task.FromResult<ErrorOr<WeatherDto>>(Error.Validation(
                code: "Weather.InvalidLongitude",
                description: "Longitude must be between -180 and 180."));
        }

        return weatherProviderClient.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);
    }
}
