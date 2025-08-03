using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace PagerDutyClient;

public class ApiKeyHttpMessageHandler : DelegatingHandler
{
    private readonly PagerDutyOptions _options;

    public ApiKeyHttpMessageHandler(IOptions<PagerDutyOptions> options)
    {
        _options = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_options.ApiKey) && request.Headers.Authorization == null)
        {
            // PagerDuty expects the format "Token token=<apiKey>"
            request.Headers.TryAddWithoutValidation("Authorization", $"Token token={_options.ApiKey}");
        }
        return base.SendAsync(request, cancellationToken);
    }
}
