using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GitHubClient;

public class GitHubClientImpl : IGitHubClient
{
    private readonly HttpClient _httpClient;
    private readonly GitHubOptions _options;

    public GitHubClientImpl(HttpClient httpClient, IOptions<GitHubOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        if (_httpClient.BaseAddress == null)
            _httpClient.BaseAddress = new System.Uri(_options.BaseUrl);
        if (!_httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("metricsclientsample"))
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("metricsclientsample");
        }
    }

    public async Task<GitHubRepo?> GetRepoAsync(string owner, string repo)
    {
        var response = await _httpClient.GetAsync($"repos/{owner}/{repo}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GitHubRepo>();
    }

    public async Task<string> GetRawAsync(string path)
    {
        var response = await _httpClient.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
