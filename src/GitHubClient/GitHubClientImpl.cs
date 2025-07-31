using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GitHubClient;

public class GitHubClientImpl : IGitHubClient
{
    private readonly HttpClient _httpClient;

    public GitHubClientImpl(HttpClient httpClient)
    {
        _httpClient = httpClient;
        if (_httpClient.BaseAddress == null)
            _httpClient.BaseAddress = new System.Uri("https://api.github.com/");
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
}
