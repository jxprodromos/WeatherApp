using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WeatherApp.Application.Services;
using WeatherApp.Infrastructure.Weather;

namespace WeatherApp.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<OpenWeatherOptions>()
            .Bind(configuration.GetSection(OpenWeatherOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<OpenWeatherApiKeyHandler>();

        services.AddHttpClient<IWeatherProviderClient, OpenWeatherClient>((serviceProvider, client) =>
        {
            OpenWeatherOptions options = serviceProvider.GetRequiredService<IOptions<OpenWeatherOptions>>().Value;
            client.BaseAddress = new Uri(options.Url, UriKind.Absolute);
            client.Timeout = TimeSpan.FromSeconds(10);
        })
        .AddHttpMessageHandler<OpenWeatherApiKeyHandler>()
        .AddStandardResilienceHandler();

        return services;
    }
}
