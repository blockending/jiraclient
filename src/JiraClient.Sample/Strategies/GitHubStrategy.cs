using System;
using System.Net.Http;

namespace JiraClient.Sample.Strategies;

public class GitHubStrategy : IApiClientStrategy
{
    private readonly HttpClient _httpClient;

    public GitHubStrategy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RunAsync()
    {
        var response = await _httpClient.GetAsync("repos/dotnet/runtime");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
    }
}
