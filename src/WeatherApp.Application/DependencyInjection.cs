using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Application.Jobs;
using WeatherApp.Application.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<WeatherFetchJob>();

        return services;
    }
}
