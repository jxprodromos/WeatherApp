namespace WeatherApp.Infrastructure.Weather;

/// <summary>
/// Custom exception thrown when the circuit breaker for OpenWeather API is open.
/// </summary>
internal sealed class CircuitBreakerOpenException : InvalidOperationException
{
    public CircuitBreakerOpenException() : base("The OpenWeather API circuit breaker is currently open. Service is temporarily unavailable.")
    {
    }

    public CircuitBreakerOpenException(string message) : base(message)
    {
    }

    public CircuitBreakerOpenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}