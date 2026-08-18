using Microsoft.OpenApi;
using WeatherApp.Api.Endpoints;
using WeatherApp.Api.Infrastructure;
using WeatherApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<HangfireDashboardAuthorizationFilter>();
builder.Services.AddHangfireInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Weather API",
        Version = "v1",
        Description = "API for retrieving the current weather for a set of coordinates."
    });
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Weather API";
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API v1");
    });
}

app.MapWeatherEndpoints();

app.UseHangfireInfrastructure();

app.Run();
