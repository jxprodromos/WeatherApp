using System.Collections.Specialized;
using Microsoft.Extensions.Options;

namespace WeatherApp.Infrastructure.Weather;

internal sealed class OpenWeatherApiKeyHandler : DelegatingHandler
{
    private readonly OpenWeatherOptions options;

    public OpenWeatherApiKeyHandler(IOptions<OpenWeatherOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        this.options = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.RequestUri);

        UriBuilder uriBuilder = new(request.RequestUri);
        NameValueCollection query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query["appid"] = options.ApiKey;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        return base.SendAsync(request, cancellationToken);
    }
}
