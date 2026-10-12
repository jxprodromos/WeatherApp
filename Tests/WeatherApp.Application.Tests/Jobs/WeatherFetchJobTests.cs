using FakeItEasy;
using Shouldly;
using Xunit;
using WeatherApp.Application.Jobs;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;
using Microsoft.Extensions.Logging;

namespace WeatherApp.Application.Tests.Jobs;

public class WeatherFetchJobTests
{
    [Fact(DisplayName = "Given valid weather response When running job Then calls provider with correct coordinates")]
    public async Task GivenValidWeatherResponse_WhenRunningJob_ThenCallsProviderWithCorrectCoordinates()
    {
        // Arrange
        var weatherDto = new WeatherDto
        {
            LocationName = "Athens",
            MainWeather = new MainWeatherInfo
            {
                TemperatureCelsius = 22.5,
                FeelsLikeCelsius = 21.0,
                PressureHpa = 1013,
                HumidityPercent = 65,
            },
            WeatherConditionInfos =
            [
                new WeatherConditionInfo
                {
                    ConditionGroup = "Clear",
                    ConditionDescription = "clear sky",
                    IconCode = "01d",
                }
            ],
            Wind = new WindDetails
            {
                SpeedMetersPerSecond = 3.5,
                DirectionDegrees = 180,
            },
            System = new SystemDetails
            {
                CountryCode = "GR",
                SunriseTimestampUtc = 1704067200,
                SunsetTimestampUtc = 1704110400,
            },
            ObservationTimestampUtc = 1704088800,
        };

        IWeatherProviderClient fakeProvider = A.Fake<IWeatherProviderClient>();
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(
                A<double>._, 
                A<double>._, 
                A<CancellationToken>._))
            .Returns(weatherDto);

        ILogger<WeatherFetchJob> fakeLogger = A.Fake<ILogger<WeatherFetchJob>>();
        WeatherFetchJob job = new(fakeProvider, fakeLogger);

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        await job.RunAsync(cancellationToken);

        // Assert
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(
                A<double>.That.Matches(lat => Math.Abs(lat - 37.9838) < 0.0001),
                A<double>.That.Matches(lon => Math.Abs(lon - 23.7275) < 0.0001),
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact(DisplayName = "Given provider error When running job Then does not throw")]
    public async Task GivenProviderError_WhenRunningJob_ThenDoesNotThrow()
    {
        // Arrange
        ErrorOr.ErrorOr<WeatherDto> errorResult = ErrorOr.Error.Failure(
            "WeatherProvider.Unavailable",
            "The weather provider is currently unavailable.");

        IWeatherProviderClient fakeProvider = A.Fake<IWeatherProviderClient>();
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(
                A<double>._, 
                A<double>._, 
                A<CancellationToken>._))
            .Returns(errorResult);

        ILogger<WeatherFetchJob> fakeLogger = A.Fake<ILogger<WeatherFetchJob>>();
        WeatherFetchJob job = new(fakeProvider, fakeLogger);

        // Act & Assert (should not throw)
        var exception = await Record.ExceptionAsync(async () =>
        {
            await job.RunAsync(CancellationToken.None);
        });

        exception.ShouldBeNull("Job should handle provider errors gracefully without throwing");
    }
}
