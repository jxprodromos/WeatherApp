using System.Collections.Specialized;
using Microsoft.Extensions.Options;
using WeatherApp.Api.Options;

public sealed class ApiKeyHandler : DelegatingHandler
{
    private readonly WeatherApiCredentialsOptions _options;

    public ApiKeyHandler(IOptions<WeatherApiCredentialsOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        UriBuilder uriBuilder = new(request.RequestUri!);
        NameValueCollection query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query["appid"] = _options.ApiKey;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}