# Coding Conventions

## Core Sections (Required)

### 1) Naming Rules

| Item | Rule | Example | Evidence |
|------|------|---------|----------|
| Files | PascalCase C# file names. | `WeatherService.cs`, `OpenWeatherClient.cs` | `WeatherApp.Application/Services/WeatherService.cs` |
| Types | PascalCase for classes and interfaces; interfaces start with `I`. | `WeatherService`, `IWeatherProviderClient` | `WeatherApp.Application/Services/IWeatherProviderClient.cs` |
| Methods | PascalCase; async methods use the `Async` suffix. | `GetCurrentWeatherAsync`, `MapWeatherEndpoints` | `WeatherApp.Application/Services/IWeatherService.cs` |
| Properties and public members | PascalCase. | `LocationName`, `TemperatureCelsius` | `WeatherApp.Application/Models/WeatherDto.cs` |
| Local variables and parameters | camelCase. | `latitude`, `longitude`, `weatherProviderClient` | `WeatherApp.Application/Services/WeatherService.cs` |
| Private fields | `_camelCase` for private fields is preferred by analyzer rules. | `_weatherProviderClient` style | `.editorconfig` |
| Private static fields | `_sCamelCase` is preferred by analyzer rules. | `_sSomething` style | `.editorconfig` |

### 2) Formatting and Language Preferences

- Indentation is 4 spaces for C# files and 2 spaces for XML files.
- End-of-line style for C# is CRLF, and the editorconfig does not add a final newline.
- `System` usings are sorted first, and using directives are placed outside namespaces.
- Braces are preferred, and line length is set to 100 characters.
- Explicit types are preferred over `var` in most cases.
- Primary constructors are preferred by analyzer rule, but the sampled source still uses explicit constructors.
- File-scoped namespaces are used in the sampled source.
- Run commands: `[TODO] No repository-defined lint or format command was found.`

### 3) Import and Module Conventions

- Import grouping/order: `System` directives are sorted first and imports are not split into separate groups.
- Alias usage: no custom aliasing pattern was found in the sampled source.
- Project structure: layer-based namespaces and references are used instead of shared barrel files or re-export modules.

### 4) Error and Logging Conventions

- Input validation uses early guards such as `ArgumentNullException.ThrowIfNull(...)`.
- Application and infrastructure methods return `ErrorOr<T>` with explicit error codes such as `Weather.InvalidLatitude` and `WeatherProvider.RequestFailed`.
- The API maps `ErrorOr` outcomes to validation problems or problem details responses.
- Provider logging uses structured messages with latitude, longitude, status code, and failure mode context.
- No repository file showed API key logging or other secret exposure in logs.

### 5) Testing Conventions

- Tests live under `Tests/<Layer>.Tests/` and use `*Tests.cs` file names.
- Test methods often follow the `Given..._When..._Then...` naming pattern.
- xUnit is used with Shouldly assertions and FakeItEasy test doubles.
- Custom `HttpMessageHandler` test doubles are used to isolate `HttpClient` behavior without network calls.
- Coverage thresholds or repo-defined coverage tooling were not found.

### 6) Evidence

- `.editorconfig`
- `WeatherApp.Application/Services/IWeatherService.cs`
- `WeatherApp.Application/Services/IWeatherProviderClient.cs`
- `WeatherApp.Application/Services/WeatherService.cs`
- `WeatherApp.Application/Models/WeatherDto.cs`
- `WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs`
- `src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs`
- `Tests/WeatherApp.Application.Tests/Services/WeatherServiceTests.cs`
- `Tests/WeatherApp.Infrastructure.Tests/Weather/OpenWeatherClientResilienceTests.cs`