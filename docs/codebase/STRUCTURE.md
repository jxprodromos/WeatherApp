# Codebase Structure

## Core Sections (Required)

### 1) Top-Level Map

List only meaningful top-level directories and files.

| Path | Purpose | Evidence |
|------|---------|----------|
| `src/WeatherApp.Api/` | Minimal API host (HTTP transport + OpenAPI + endpoint mapping). | src/WeatherApp.Api/Program.cs |
| `WeatherApp.Application/` | Business rules and orchestration (validation and service contracts). | WeatherApp.Application/Services/WeatherService.cs |
| `WeatherApp.Infrastructure/` | External provider integration, HTTP pipeline, mapping, options binding. | WeatherApp.Infrastructure/DependencyInjection.cs |
| `Tests/WeatherApp.Application.Tests/` | Application-layer unit tests. | Tests/WeatherApp.Application.Tests/Services/WeatherServiceTests.cs |
| `Tests/WeatherApp.Infrastructure.Tests/` | Infrastructure-layer unit/resilience mapping tests. | Tests/WeatherApp.Infrastructure.Tests/Weather/OpenWeatherClientResilienceTests.cs |
| `.github/` | Repository instructions and acquire-codebase-knowledge skill assets. | .github/copilot-instructions.md |
| `docs/codebase/` | Generated onboarding knowledge documents and scanner output. | docs/codebase/.codebase-scan.txt |
| `WeatherApp.slnx` | Solution-level project composition. | WeatherApp.slnx |

### 2) Entry Points

- Main runtime entry: `src/WeatherApp.Api/Program.cs`
- Secondary entry points (worker/cli/jobs): `NONE`
- How entry is selected (script/config): web host bootstraps from API project startup profile in `src/WeatherApp.Api/Properties/launchSettings.json`.

### 3) Module Boundaries

| Boundary | What belongs here | What must not be here |
|----------|-------------------|------------------------|
| API (`src/WeatherApp.Api`) | Route definitions, query binding, response shaping/OpenAPI metadata. | Provider HTTP details and provider DTO mapping logic. |
| Application (`WeatherApp.Application`) | Input validation, use-case orchestration, abstraction of weather provider dependency. | Direct HTTP calls and provider-specific transport details. |
| Infrastructure (`WeatherApp.Infrastructure`) | Typed HttpClient, resilience handlers, provider auth/query wiring, raw-to-domain mapping. | HTTP route concerns and API transport metadata. |
| Tests (`Tests/*`) | Unit tests targeting a single layer per test project. | Production runtime registration/entrypoint logic. |

### 4) Naming and Organization Rules

- File naming pattern: PascalCase for C# source files (e.g., `WeatherService.cs`, `OpenWeatherClient.cs`).
- Directory organization pattern: layer-based (`Api`, `Application`, `Infrastructure`, `Tests`).
- Import aliasing or path conventions: standard namespace imports; no custom alias/path mapping files found.

### 5) Evidence

- docs/codebase/.codebase-scan.txt
- WeatherApp.slnx
- src/WeatherApp.Api/Program.cs
- src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs
- WeatherApp.Application/Services/WeatherService.cs
- WeatherApp.Infrastructure/DependencyInjection.cs