using Shouldly;
using Xunit;
using WeatherApp.Application.Models;
using WeatherApp.Infrastructure.Weather;

namespace WeatherApp.Infrastructure.Tests.Weather;

public class OpenWeatherMapperTests
{
    [Fact(DisplayName = "Given a complete OpenWeather response When converting to DTO Then all fields are mapped correctly")]
    public void GivenCompleteOpenWeatherResponse_WhenConvertingToDto_ThenAllFieldsAreMappedCorrectly()
    {
        // Arrange
        long sunrise = 1704067200L;
        long sunset = 1704110400L;
        long observedAt = 1704088800L;

        OpenWeatherResponse response = new()
        {
            Name = "Athens",
            Main = new OpenWeatherResponse.MainResponse
            {
                Temp = 15.5,
                FeelsLike = 14.2,
                Pressure = 1013,
                Humidity = 75,
            },
            Weather =
            [
                new OpenWeatherResponse.WeatherResponse
                {
                    Main = "Clouds",
                    Description = "overcast clouds",
                    Icon = "04d",
                }
            ],
            Wind = new OpenWeatherResponse.WindResponse
            {
                Speed = 3.5,
                Deg = 45,
            },
            Sys = new OpenWeatherResponse.SysResponse
            {
                Country = "GR",
                Sunrise = sunrise,
                Sunset = sunset,
            },
            Dt = observedAt,
        };

        // Act
        WeatherDto result = OpenWeatherMapper.ToDto(response);

        // Assert
        result.LocationName.ShouldBe("Athens");

        result.MainWeather.TemperatureCelsius.ShouldBe(15.5);
        result.MainWeather.FeelsLikeCelsius.ShouldBe(14.2);
        result.MainWeather.PressureHpa.ShouldBe(1013);
        result.MainWeather.HumidityPercent.ShouldBe(75);

        result.WeatherConditionInfos.Count.ShouldBe(1);
        result.WeatherConditionInfos[0].ConditionGroup.ShouldBe("Clouds");
        result.WeatherConditionInfos[0].ConditionDescription.ShouldBe("overcast clouds");
        result.WeatherConditionInfos[0].IconCode.ShouldBe("04d");

        result.Wind.SpeedMetersPerSecond.ShouldBe(3.5);
        result.Wind.DirectionDegrees.ShouldBe(45);

        result.System.CountryCode.ShouldBe("GR");
        result.System.SunriseTimestampUtc.ShouldBe(sunrise);
        result.System.SunsetTimestampUtc.ShouldBe(sunset);

        result.ObservationTimestampUtc.ShouldBe(observedAt);
    }

    [Fact(DisplayName = "Given an OpenWeather response with empty conditions When converting to DTO Then unfilled conditions contain empty strings")]
    public void GivenOpenWeatherResponseWithEmptyWeatherList_WhenConvertingToDto_ThenConditionFieldsAreEmpty()
    {
        // Arrange
        OpenWeatherResponse response = new()
        {
            Name = "TestCity",
            Main = new OpenWeatherResponse.MainResponse
            {
                Temp = 20.0,
                FeelsLike = 19.0,
                Pressure = 1010,
                Humidity = 60,
            },
            Weather = [],
            Wind = new OpenWeatherResponse.WindResponse
            {
                Speed = 2.0,
                Deg = 180,
            },
            Sys = new OpenWeatherResponse.SysResponse
            {
                Country = "US",
                Sunrise = null,
                Sunset = null,
            },
            Dt = 1704000000L,
        };

        // Act
        WeatherDto result = OpenWeatherMapper.ToDto(response);

        // Assert
        result.WeatherConditionInfos.Count.ShouldBe(1);
        result.WeatherConditionInfos[0].ConditionGroup.ShouldBe(string.Empty);
        result.WeatherConditionInfos[0].ConditionDescription.ShouldBe(string.Empty);
        result.WeatherConditionInfos[0].IconCode.ShouldBe(string.Empty);

        result.System.SunriseTimestampUtc.ShouldBeNull();
        result.System.SunsetTimestampUtc.ShouldBeNull();
    }
}
