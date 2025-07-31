using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace JiraClient;

public class JiraOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public OAuthOptions OAuth { get; set; } = new();
}

public class JiraClientImpl : IJiraClient
{
    private readonly HttpClient _httpClient;
    private readonly JiraOptions _options;

    public JiraClientImpl(HttpClient httpClient, IOptions<JiraOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _httpClient.BaseAddress = new System.Uri(_options.BaseUrl);
    }

    public async Task<JiraIssue> GetIssueAsync(string issueKey)
    {
        var response = await _httpClient.GetAsync($"/rest/api/2/issue/{issueKey}");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        var issue = await JsonSerializer.DeserializeAsync<JiraIssue>(stream) ?? new JiraIssue();
        return issue;
    }
}
