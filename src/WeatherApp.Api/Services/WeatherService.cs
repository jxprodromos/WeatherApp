using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<WeatherService> logger;

    public WeatherService(IHttpClientFactory httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task<WeatherResult> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        //weather?lat=48.2082&lon=16.3738
        HttpClient client = _httpClient.CreateClient();

        string apiKey = configuration["OpenWeatherMap:ApiKey"]!;

        var response = await client.GetAsync(
            $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<WeatherResult>(
            cancellationToken: cancellationToken);

        return result!;
    }
}