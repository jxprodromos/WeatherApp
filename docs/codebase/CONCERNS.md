# Codebase Concerns

## Core Sections (Required)

### 1) Top Risks (Prioritized)

| Severity | Concern | Evidence | Impact | Suggested action |
|----------|---------|----------|--------|------------------|
| high | Weather endpoint has no visible authentication or rate limiting. | src/WeatherApp.Api/Program.cs; src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs | Service abuse and provider quota exhaustion risk. | Add explicit auth/rate-limit policy based on product intent. |
| medium | Integration observability lacks metrics/tracing. | WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs | Harder incident diagnosis and capacity planning. | Add OpenTelemetry metrics/traces for outbound requests and failures. |
| medium | Provider base URL and key are required but deployment source of truth is undocumented. | src/WeatherApp.Api/appsettings.json; WeatherApp.Infrastructure/Weather/OpenWeatherOptions.cs | Environment drift and failed startups in new environments. | Document environment provisioning and secret management flow. |

### 2) Technical Debt

List the most important debt items only.

| Debt item | Why it exists | Where | Risk if ignored | Suggested fix |
|-----------|---------------|-------|-----------------|---------------|
| Unused custom exception type | Legacy/partial implementation from resilience evolution. | WeatherApp.Infrastructure/Weather/CircuitBreakerOpenException.cs | Misleading maintenance signal; dead code growth. | Remove if unused or wire it into actual exception mapping strategy. |
| Sparse project intent documentation | README currently has only project title. | README.md | Slower onboarding and inconsistent implementation assumptions. | Expand README with architecture intent, setup, and operational notes. |
| No automated API contract tests | Current tests focus on Application/Infrastructure internals only. | Tests/ (absence of API test project) | Endpoint regressions may ship undetected. | Add API-level tests for `/v1/weather` contract/status mapping. |

### 3) Security Concerns

| Risk | OWASP category (if applicable) | Evidence | Current mitigation | Gap |
|------|--------------------------------|----------|--------------------|-----|
| Missing endpoint access controls | A01: Broken Access Control (context-dependent) | src/WeatherApp.Api/Program.cs | None visible in startup pipeline. | [ASK USER] confirm whether API is intended public/anonymous. |
| Secret exposure risk from config misuse | A02: Cryptographic Failures (credential handling) | src/WeatherApp.Api/appsettings.json | API key is empty in repo and bound via options. | No documented secret rotation/storage standard in repo. |

### 4) Performance and Scaling Concerns

| Concern | Evidence | Current symptom | Scaling risk | Suggested improvement |
|---------|----------|-----------------|-------------|-----------------------|
| Every request makes synchronous external API dependency call | WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs | Latency is tied to upstream provider response time. | Throughput degradation under provider slowness/outage. | Consider caching or fallback strategy for repeated coordinate queries. |
| Potential high provider-call volume without ingress throttling | src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs | No visible request throttling at API layer. | Quota exhaustion and amplified failure during traffic spikes. | Add ASP.NET rate limiting and quota-aware backpressure. |

### 5) Fragile/High-Churn Areas

| Area | Why fragile | Churn signal | Safe change strategy |
|------|-------------|-------------|----------------------|
| `WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs` | Encodes provider error translation + logging + resilience touchpoints. | Listed in scan high-churn files (4 changes). | Add targeted tests before modifying mapping/failure behavior. |
| `src/WeatherApp.Api/Program.cs` | Startup composition affects DI, middleware, and OpenAPI behavior. | Listed in scan high-churn files (4 changes). | Keep startup diffs minimal and validate with endpoint smoke tests. |
| `src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs` | Owns contract response mapping and status code translation. | Listed in scan high-churn files (2 changes). | Preserve OpenAPI metadata and error code families when editing. |

### 6) `[ASK USER]` Questions

Add unresolved intent-dependent questions as a numbered list.

1. [ASK USER] Should `/v1/weather` remain publicly anonymous, or should authentication/authorization be added?
2. [ASK USER] Is resilience behavior from `AddStandardResilienceHandler()` sufficient, or do you require explicit retry/circuit-breaker policy values documented and enforced?
3. [ASK USER] Do you want API-level contract tests added as a repository standard for endpoint changes?

### 7) Evidence

- docs/codebase/.codebase-scan.txt
- src/WeatherApp.Api/Program.cs
- src/WeatherApp.Api/Endpoints/WeatherEndpoints.cs
- src/WeatherApp.Api/appsettings.json
- WeatherApp.Infrastructure/Weather/OpenWeatherClient.cs
- WeatherApp.Infrastructure/Weather/OpenWeatherOptions.cs
- WeatherApp.Infrastructure/Weather/CircuitBreakerOpenException.cs
- README.md