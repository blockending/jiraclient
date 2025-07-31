using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace JiraClient;

public interface IOAuthTokenProvider
{
    Task<string> GetAccessTokenAsync();
}

internal class OAuthTokenProvider : IOAuthTokenProvider
{
    private class TokenResponse
    {
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; }
    }

    private readonly HttpClient _httpClient;
    private readonly OAuthOptions _options;
    private string? _accessToken;
    private DateTime _expiresAtUtc;

    public OAuthTokenProvider(HttpClient httpClient, IOptions<OAuthOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken == null || DateTime.UtcNow >= _expiresAtUtc)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = _options.RefreshToken,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret
            });

            var response = await _httpClient.PostAsync(_options.TokenUrl, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenResponse>(json)!;
            _accessToken = token.access_token;
            _expiresAtUtc = DateTime.UtcNow.AddSeconds(token.expires_in - 30);
        }

        return _accessToken!;
    }
}
