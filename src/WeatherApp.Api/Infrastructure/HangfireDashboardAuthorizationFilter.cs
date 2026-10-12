using Hangfire.Dashboard;

namespace WeatherApp.Api.Infrastructure;

/// <summary>
/// Authorization filter for Hangfire Dashboard.
/// Allows access in Development environment only; denies access in Production by default.
/// Production deployment should implement proper authentication (e.g., API key, JWT, Basic Auth).
/// </summary>
internal sealed class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly IWebHostEnvironment _environment;

    public HangfireDashboardAuthorizationFilter(IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);
        _environment = environment;
    }

    public bool Authorize(DashboardContext context)
    {
        // Development: allow access
        // Production: deny (fail-closed, safe default)
        return _environment.IsDevelopment();
    }
}
