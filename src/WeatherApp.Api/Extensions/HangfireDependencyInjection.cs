using Hangfire;
using Hangfire.SqlServer;
using WeatherApp.Api.Infrastructure;
using WeatherApp.Application.Jobs;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Hangfire dependency injection extension methods.
/// </summary>
internal static class HangfireDependencyInjection
{
    public static IServiceCollection AddHangfireInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        string? hangfireConnection = configuration.GetConnectionString("HangfireConnection");
        if (string.IsNullOrWhiteSpace(hangfireConnection))
        {
            throw new InvalidOperationException("HangfireConnection connection string is required but not found in configuration.");
        }

        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnection, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    DisableGlobalLocks = true,
                    SchemaName = "Hangfire",
                });
        });

        services.AddHangfireServer();

        return services;
    }

    public static WebApplication UseHangfireInfrastructure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var authorizationFilter = app.Services.GetRequiredService<HangfireDashboardAuthorizationFilter>();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { authorizationFilter },
            IgnoreAntiforgeryToken = true, // Safe in local/dev context; production will need proper CSRF handling
        });

        // Register the recurring weather fetch job
        // Cron: "*/15 * * * *" = every 15 minutes
        try
        {
            RecurringJob.AddOrUpdate<WeatherFetchJob>(
                "weather-fetch-job",
                job => job.RunAsync(CancellationToken.None),
                "*/15 * * * *");
        }
        catch (Exception ex)
        {
            // Log error but allow app to continue; the job will be registered when the database becomes available
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Failed to register recurring job 'weather-fetch-job'. The job may not execute until the database is available.");
        }

        return app;
    }
}
