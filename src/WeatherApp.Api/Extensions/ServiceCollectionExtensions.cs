using Microsoft.Extensions.Options;
using WeatherApp.Api.Options;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<WeatherApiCredentialsOptions>()
            .Bind(configuration.GetSection(WeatherApiCredentialsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<WeatherApiCredentialsOptions>()
            .Bind(configuration.GetSection(WeatherApiCredentialsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<ApiKeyHandler>();

        services.AddHttpClient<IWeatherService, WeatherService>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<WeatherApiCredentialsOptions>>().Value;
            client.BaseAddress = new Uri(options.Url);
        })
        .AddHttpMessageHandler<ApiKeyHandler>();

        return services;
    }
}
