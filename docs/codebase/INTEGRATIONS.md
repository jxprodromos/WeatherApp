# External Integrations

## Core Sections (Required)

### 1) Integration Inventory

| System | Type (API/DB/Queue/etc) | Purpose | Auth model | Criticality | Evidence |
|--------|---------------------------|---------|------------|-------------|----------|
| OpenWeatherMap | External HTTP API | Source of current weather data by coordinates. | API key passed as `appid` query parameter via delegating handler. | High | WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs |

### 2) Data Stores

| Store | Role | Access layer | Key risk | Evidence |
|-------|------|--------------|----------|----------|
| None found | N/A | N/A | Forecast/history persistence is unavailable in current design. | docs/codebase/.codebase-scan.txt |

### 3) Secrets and Credentials Handling

- Credential sources: `OpenWeatherMap:ApiKey` from configuration binding (`appsettings`/user secrets/env vars).
- Hardcoding checks: no hardcoded API key value in repo (`appsettings.json` keeps it empty).
- Rotation or lifecycle notes: `[TODO] No credential rotation process documented in repository.`

### 4) Reliability and Failure Behavior

- Retry/backoff behavior: implemented through `AddStandardResilienceHandler()` on typed HttpClient.
- Timeout policy: `HttpClient.Timeout = TimeSpan.FromSeconds(10)` in infrastructure registration.
- Circuit-breaker or fallback behavior: circuit-breaker behavior is implied by standard resilience pipeline; provider maps `BrokenCircuitException` to `WeatherProvider.CircuitOpen`.

### 5) Observability for Integrations

- Logging around external calls: yes; information/warning/error logs in `OpenWeatherClient` with coordinate/status context.
- Metrics/tracing coverage: no explicit metrics or tracing hooks found.
- Missing visibility gaps: no evident distributed tracing/counters for external request latency and failure rates.

### 6) Evidence

- WeatherApp.Infrastructure/DependencyInjection.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherApiKeyHandler.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherOptions.cs
- src/WeatherApp.Api/appsettings.json
- docs/codebase/.codebase-scan.txt