using ErrorOr;
using FakeItEasy;
using Shouldly;
using Xunit;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;

namespace WeatherApp.Application.Tests.Services;

public class WeatherServiceTests
{
    [Fact(DisplayName = "Given valid coordinates When getting current weather Then provider is called and returns weather")]
    public async Task GivenValidCoordinates_WhenGetCurrentWeather_ThenProviderIsCalledAndReturnsWeather()
    {
        // Arrange
        double latitude = 37.9838;
        double longitude = 23.7275;
        CancellationToken cancellationToken = CancellationToken.None;

        WeatherDto expectedWeatherDto = new()
        {
            LocationName = "Athens",
            MainWeather = new MainWeatherInfo
            {
                TemperatureCelsius = 20.0,
                FeelsLikeCelsius = 19.5,
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
                SpeedMetersPerSecond = 1.5,
                DirectionDegrees = 90,
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
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(latitude, longitude, cancellationToken))
            .Returns(expectedWeatherDto);

        WeatherService service = new(fakeProvider);

        // Act
        ErrorOr<WeatherDto> result = await service.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(expectedWeatherDto);
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(latitude, longitude, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(-91)]
    [InlineData(91)]
    [InlineData(-150)]
    [InlineData(150)]
    public async Task GivenInvalidLatitude_WhenGetCurrentWeather_ThenReturnsValidationErrorWithoutCallingProvider(double invalidLatitude)
    {
        // Arrange
        double longitude = 23.7275;
        CancellationToken cancellationToken = CancellationToken.None;
        IWeatherProviderClient fakeProvider = A.Fake<IWeatherProviderClient>();
        WeatherService service = new(fakeProvider);

        // Act
        ErrorOr<WeatherDto> result = await service.GetCurrentWeatherAsync(invalidLatitude, longitude, cancellationToken);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe("Weather.InvalidLatitude");
        result.Errors[0].Type.ShouldBe(ErrorType.Validation);
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(A<double>._, A<double>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Theory]
    [InlineData(-181)]
    [InlineData(181)]
    [InlineData(-270)]
    [InlineData(270)]
    public async Task GivenInvalidLongitude_WhenGetCurrentWeather_ThenReturnsValidationErrorWithoutCallingProvider(double invalidLongitude)
    {
        // Arrange
        double latitude = 37.9838;
        CancellationToken cancellationToken = CancellationToken.None;
        IWeatherProviderClient fakeProvider = A.Fake<IWeatherProviderClient>();
        WeatherService service = new(fakeProvider);

        // Act
        ErrorOr<WeatherDto> result = await service.GetCurrentWeatherAsync(latitude, invalidLongitude, cancellationToken);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Code.ShouldBe("Weather.InvalidLongitude");
        result.Errors[0].Type.ShouldBe(ErrorType.Validation);
        A.CallTo(() => fakeProvider.GetCurrentWeatherAsync(A<double>._, A<double>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}
