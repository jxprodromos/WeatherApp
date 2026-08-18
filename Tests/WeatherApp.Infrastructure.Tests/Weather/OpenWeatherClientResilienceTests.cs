using FakeItEasy;
using Shouldly;
using Xunit;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;
using WeatherApp.Infrastructure.Weather;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WeatherApp.Infrastructure.Tests.Weather;

public class OpenWeatherClientResilienceTests
{
    [Fact(DisplayName = "Given 503 Service Unavailable When getting weather Then should fail immediately without retry")]
    public async Task Given503ServiceUnavailable_WhenGetCurrentWeather_ThenReturnsFailure()
    {
        // Arrange
        double latitude = 37.9838;
        double longitude = 23.7275;
        CancellationToken cancellationToken = CancellationToken.None;

        var handler = new TestHttpMessageHandler(httpRequestMessage =>
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.openweathermap.org/", UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(10)
        };

        ILogger<OpenWeatherClient> logger = A.Fake<ILogger<OpenWeatherClient>>();
        OpenWeatherClient client = new(httpClient, logger);

        // Act
        var result = await client.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Errors[0].Code.ShouldBe("WeatherProvider.RequestFailed");
    }

    [Fact(DisplayName = "Given 400 Bad Request When getting weather Then returns failure")]
    public async Task Given400BadRequest_WhenGetCurrentWeather_ThenReturnsFailure()
    {
        // Arrange
        double latitude = 37.9838;
        double longitude = 23.7275;
        CancellationToken cancellationToken = CancellationToken.None;

        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadRequest));

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.openweathermap.org/", UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(10)
        };

        ILogger<OpenWeatherClient> logger = A.Fake<ILogger<OpenWeatherClient>>();
        OpenWeatherClient client = new(httpClient, logger);

        // Act
        var result = await client.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Errors[0].Code.ShouldBe("WeatherProvider.RequestFailed");
    }

    private sealed class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> responseFactory;

        public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            this.responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request));
        }
    }
}
