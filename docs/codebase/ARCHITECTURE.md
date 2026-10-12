# Architecture

## Core Sections (Required)

### 1) Architectural Style

- Primary style: Layered architecture (API -> Application -> Infrastructure).
- Why this classification: API project references Application and Infrastructure; Infrastructure references Application; Application contains service contracts/validation and no infrastructure code.
- Primary constraints:
  - API contract is exposed via Minimal API endpoint `v1/weather`.
  - Weather provider access goes through `IWeatherProviderClient` abstraction.
  - Provider resilience and options validation are configured in infrastructure DI.

### 2) System Flow

```text
HTTP GET /v1/weather -> endpoint query binding -> application validation/orchestration -> infrastructure provider HTTP call -> mapping to WeatherDto -> HTTP result
```

Flow (evidence-backed):
1. `Program.cs` builds the web app and registers endpoint mapping with `MapWeatherEndpoints()`.
2. `WeatherEndpoints.GetCurrentWeather` binds `latitude`/`longitude` and calls `IWeatherService.GetCurrentWeatherAsync(...)`.
3. `WeatherService` validates coordinate ranges and returns validation errors when out of bounds.
4. `OpenWeatherClient` (typed as `IWeatherProviderClient`) issues the OpenWeather HTTP GET and applies response-status error mapping.
5. `OpenWeatherMapper` converts provider DTO (`OpenWeatherResponse`) into application DTO (`WeatherDto`).
6. Endpoint maps `ErrorOr` outcomes into `200`, validation problem, or problem details responses.

### 3) Layer/Module Responsibilities

| Layer or module | Owns | Must not own | Evidence |
|-----------------|------|--------------|----------|
| API | HTTP route definitions and response shaping (`Results<...>` and status mapping) | Provider transport/resilience internals | src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs |
| Application | Validation and orchestration via service abstraction | Provider protocol details (`HttpClient`, URL/query/key management) | WeatherApp.Application/Services/WeatherService.cs |
| Infrastructure | OpenWeather options binding, typed HttpClient + resilience, API key handler, response mapping | Endpoint contract/OpenAPI metadata | WeatherApp.Infrastructure/DependencyInjection.cs |

### 4) Reused Patterns

| Pattern | Where found | Why it exists |
|---------|-------------|---------------|
| Dependency Injection | `AddApplication`, `AddInfrastructure`, typed `AddHttpClient` registrations | Decouple construction from behavior and enforce layer boundaries. |
| Adapter/Port pattern | `OpenWeatherClient : IWeatherProviderClient` | Isolate external provider implementation behind application-facing contract. |
| Options pattern | `OpenWeatherOptions` + `AddOptions().Bind().ValidateOnStart()` | Centralize configuration and validate required provider credentials early. |
| Resilience policy pipeline | `.AddStandardResilienceHandler()` | Improve external API call robustness under transient failures. |

### 5) Known Architectural Risks

- `CircuitBreakerOpenException` exists but is not referenced by current flow; dead code can mislead future maintenance.
- No auth/rate-limiting guard is visible on `v1/weather`, so public abuse controls appear absent in current architecture.

### 6) Evidence

- WeatherApp.slnx
- src/WeatherApp.Api/Program.cs
- src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs
- WeatherApp.Application/Services/IWeatherProviderClient.cs
- WeatherApp.Application/Services/WeatherService.cs
- WeatherApp.Infrastructure/DependencyInjection.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherMapper.cs
- WeatherApp.Infrastructure/Weather/CircuitBreakerOpenException.cs