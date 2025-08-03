using System.Net.Http;
using System.Net.Http.Headers;

namespace JiraClient;

public class OAuthHttpMessageHandler : DelegatingHandler
{
    private readonly IOAuthTokenProvider _tokenProvider;

    public OAuthHttpMessageHandler(IOAuthTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}
