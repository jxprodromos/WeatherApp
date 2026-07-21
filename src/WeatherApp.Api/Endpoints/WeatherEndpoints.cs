using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;

namespace WeatherApp.Api.Endpoints;

public static class WeatherEndpoints
{
    public static RouteGroupBuilder MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints.MapGroup("v1/weather")
            .WithTags("Weather");

        group.MapGet(string.Empty, GetCurrentWeather)
            .WithName("GetCurrentWeather")
            .WithSummary("Gets the current weather for the supplied coordinates.")
            .WithDescription("Returns a single current weather record for the latitude and longitude query string values.")
            .Produces<WeatherDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status502BadGateway)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi();

        return group;
    }

    private static async Task<Results<Ok<WeatherDto>, ValidationProblem, ProblemHttpResult>> GetCurrentWeather(
        [FromQuery] double lat,
        [FromQuery] double lon,
        [FromServices] IWeatherService weatherService,
        CancellationToken cancellationToken)
    {
        ErrorOr<WeatherDto> result = await weatherService.GetCurrentWeatherAsync(lat, lon, cancellationToken);

        return result.Match<Results<Ok<WeatherDto>, ValidationProblem, ProblemHttpResult>>(
            weather => TypedResults.Ok(weather),
            errors => errors.All(static error => error.Type == ErrorType.Validation)
                ? TypedResults.ValidationProblem(CreateValidationErrors(errors))
                : TypedResults.Problem(
                    title: errors[0].Code,
                    detail: errors[0].Description,
                    statusCode: MapStatusCode(errors[0].Type)));
    }

    private static Dictionary<string, string[]> CreateValidationErrors(List<Error> errors)
    {
        return errors
            .GroupBy(static error => error.Code)
            .ToDictionary(
                static group => group.Key,
                static group => group.Select(static error => error.Description).ToArray());
    }

    private static int MapStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Failure => StatusCodes.Status502BadGateway,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
