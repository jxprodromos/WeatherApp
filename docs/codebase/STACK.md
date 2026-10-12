# Technology Stack

## Core Sections (Required)

### 1) Runtime Summary

| Area | Value | Evidence |
|------|-------|----------|
| Primary language | C# | `WeatherApp.Application/WeatherApp.Application.csproj` |
| Target framework | .NET 10 (`net10.0`) | `src/WeatherApp.Api/WeatherApp.Api.csproj` |
| Project style | SDK-style project files | `src/WeatherApp.Api/WeatherApp.Api.csproj` |
| Solution format | `.slnx` solution file | `WeatherApp.slnx` |
| Package management | NuGet `PackageReference` | `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj` |

### 2) Production Frameworks and Dependencies

| Dependency | Version | Role in system | Evidence |
|------------|---------|----------------|----------|
| `Microsoft.AspNetCore.OpenApi` | `10.0.10` | OpenAPI endpoint metadata and discovery | `src/WeatherApp.Api/WeatherApp.Api.csproj` |
| `Swashbuckle.AspNetCore` | `10.2.3` | Swagger document/UI generation | `src/WeatherApp.Api/WeatherApp.Api.csproj` |
| `ErrorOr` | `2.1.0` | Domain-safe result/error modeling | `WeatherApp.Application/WeatherApp.Application.csproj` |
| `Microsoft.Extensions.Http` | `10.0.10` | Typed `HttpClient` infrastructure | `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj` |
| `Microsoft.Extensions.Http.Resilience` | `10.0.10` | Standard resilience pipeline for provider calls | `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj` |
| `Microsoft.Extensions.Options.ConfigurationExtensions` | `10.0.10` | Bind provider options from configuration | `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj` |
| `Microsoft.Extensions.Options.DataAnnotations` | `10.0.10` | Validate provider options on startup | `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj` |

### 3) Development Toolchain

| Tool | Purpose | Evidence |
|------|---------|----------|
| `.editorconfig` | Formatting and analyzer rules | `.editorconfig` |
| `Microsoft.NET.Test.Sdk` | Test discovery and execution | `Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj` |
| `xunit` | Test framework | `Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj` |
| `Shouldly` | Assertions | `Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj` |
| `FakeItEasy` | Test doubles | `Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj` |

### 4) Key Commands

```bash
dotnet restore
dotnet build WeatherApp.slnx
dotnet test WeatherApp.slnx
[TODO] No repository-defined lint or format command was found.
```

### 5) Environment and Config

- Config files present in the repo: `src/WeatherApp.Api/appsettings.json` and `src/WeatherApp.Api/appsettings.Development.json`.
- Development launch profiles set `ASPNETCORE_ENVIRONMENT=Development` in `src/WeatherApp.Api/Properties/launchSettings.json`.
- `src/WeatherApp.Api/WeatherApp.Api.csproj` sets `UserSecretsId`, which indicates user secrets are available for local secret storage.
- `src/WeatherApp.Api/appsettings.json` contains an `OpenWeatherMap` section with `Url` set to `https://api.openweathermap.org/` and `ApiKey` left empty in source control.

### 6) Evidence

- `WeatherApp.slnx`
- `src/WeatherApp.Api/WeatherApp.Api.csproj`
- `WeatherApp.Application/WeatherApp.Application.csproj`
- `WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj`
- `Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj`
- `Tests/WeatherApp.Infrastructure.Tests/WeatherApp.Infrastructure.Tests.csproj`
- `src/WeatherApp.Api/appsettings.json`
- `src/WeatherApp.Api/Properties/launchSettings.json`
- `.editorconfig`