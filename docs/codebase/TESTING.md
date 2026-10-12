# Testing Patterns

## Core Sections (Required)

### 1) Test Stack and Commands

- Primary test framework: xUnit 2.9.0.
- Assertion/mocking tools: Shouldly 4.2.1, FakeItEasy 9.0.0.
- Commands:

```bash
dotnet test WeatherApp.slnx
dotnet test Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj
dotnet test Tests/WeatherApp.Infrastructure.Tests/WeatherApp.Infrastructure.Tests.csproj
[TODO] Coverage command/config is not defined in repository.
```

### 2) Test Layout

- Test file placement pattern: dedicated `Tests/` directory with per-layer test projects.
- Naming convention: test classes use `<Subject>Tests`; methods follow `Given..._When..._Then...` in many cases.
- Setup files and where they run: no shared global setup fixtures found; setup is per-test/class.

### 3) Test Scope Matrix

| Scope | Covered? | Typical target | Notes |
|-------|----------|----------------|-------|
| Unit | yes | Application service validation and infrastructure mapper/client behavior | See WeatherService and OpenWeatherMapper tests. |
| Integration | no | `[TODO]` | No tests hitting hosted API or real OpenWeather integration found. |
| E2E | no | `[TODO]` | No end-to-end flow tests found. |

### 4) Mocking and Isolation Strategy

- Main mocking approach: FakeItEasy for interfaces/loggers; custom in-memory `HttpMessageHandler` for HttpClient isolation.
- Isolation guarantees: tests avoid real network calls by injecting fake provider or fake message handler.
- Common failure mode in tests: behavior-level assertions are present, but no assertions around retry timing/telemetry coverage.

### 5) Coverage and Quality Signals

- Coverage tool + threshold: `[TODO]` No threshold/config file found.
- Current reported coverage: `[TODO]` No report artifact found in repository.
- Known gaps/flaky areas: API endpoint behavior and DI wiring have no direct automated tests in current tree.

### 6) Evidence

- Tests/WeatherApp.Application.Tests/WeatherApp.Application.Tests.csproj
- Tests/WeatherApp.Infrastructure.Tests/WeatherApp.Infrastructure.Tests.csproj
- Tests/WeatherApp.Application.Tests/Services/WeatherServiceTests.cs
- Tests/WeatherApp.Infrastructure.Tests/Weather/OpenWeatherClientResilienceTests.cs
- Tests/WeatherApp.Infrastructure.Tests/Weather/OpenWeatherMapperTests.cs
- docs/codebase/.codebase-scan.txt