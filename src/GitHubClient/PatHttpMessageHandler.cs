using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace GitHubClient;

public class PatHttpMessageHandler : DelegatingHandler
{
    private readonly GitHubOptions _options;

    public PatHttpMessageHandler(IOptions<GitHubOptions> options)
    {
        _options = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_options.PersonalAccessToken) && request.Headers.Authorization == null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("token", _options.PersonalAccessToken);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
