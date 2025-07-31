using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using JiraClient.Mapping;
using JiraClient.Sample;

namespace JiraClient.Sample.Strategies;

public class GitHubStrategy : IApiClientStrategy
{
    private readonly HttpClient _httpClient;
    private readonly DynamicMappingService _mapper;

    public GitHubStrategy(HttpClient httpClient, DynamicMappingService mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task RunAsync()
    {
        var response = await _httpClient.GetAsync("repos/dotnet/runtime");
        response.EnsureSuccessStatusCode();
        var repo = await response.Content.ReadFromJsonAsync<GitHubRepo>();
        if (repo is not null)
        {
            var mapped = _mapper.Map<UnifiedIssue>("github", repo);
            Console.WriteLine(JsonSerializer.Serialize(mapped, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
