using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Microsoft.Extensions.Logging;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;

namespace WeatherApp.Infrastructure.Weather;

public sealed class OpenWeatherClient : IWeatherProviderClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<OpenWeatherClient> logger;

    public OpenWeatherClient(HttpClient httpClient, ILogger<OpenWeatherClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<ErrorOr<WeatherDto>> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(
                $"data/2.5/weather?lat={latitude}&lon={longitude}&units=metric",
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                logger.LogInformation("Weather data was not found for coordinates {Latitude}, {Longitude}.", latitude, longitude);
                return Error.NotFound(
                    code: "Weather.NotFound",
                    description: $"Weather data for coordinates ({latitude}, {longitude}) was not found.");
            }

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "OpenWeather request failed with status code {StatusCode} for coordinates {Latitude}, {Longitude}.",
                    (int)response.StatusCode,
                    latitude,
                    longitude);

                return Error.Failure(
                    code: "WeatherProvider.RequestFailed",
                    description: "The weather provider request failed.");
            }

            OpenWeatherResponse? rawResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>(cancellationToken: cancellationToken);

            if (rawResponse is null)
            {
                logger.LogWarning("OpenWeather returned an unreadable response body for coordinates {Latitude}, {Longitude}.", latitude, longitude);
                return Error.Failure(
                    code: "WeatherProvider.NullResponse",
                    description: "Provider returned an unreadable response.");
            }

            WeatherDto result = OpenWeatherMapper.ToDto(rawResponse);

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "OpenWeather request failed for coordinates {Latitude}, {Longitude}.", latitude, longitude);
            return Error.Failure(
                code: "WeatherProvider.Unavailable",
                description: "The weather provider is currently unavailable.");
        }
        catch (NotSupportedException exception)
        {
            logger.LogError(exception, "OpenWeather returned an unsupported content type for coordinates {Latitude}, {Longitude}.", latitude, longitude);
            return Error.Unexpected(
                code: "WeatherProvider.UnsupportedContent",
                description: "The weather provider returned an unsupported response format.");
        }
        catch (System.Text.Json.JsonException exception)
        {
            logger.LogError(exception, "OpenWeather returned invalid JSON for coordinates {Latitude}, {Longitude}.", latitude, longitude);
            return Error.Unexpected(
                code: "WeatherProvider.InvalidJson",
                description: "The weather provider returned invalid data.");
        }
    }
}
