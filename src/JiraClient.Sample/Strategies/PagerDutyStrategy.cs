using System;
using System.Net.Http;

namespace JiraClient.Sample.Strategies;

public class PagerDutyStrategy : IApiClientStrategy
{
    private readonly HttpClient _httpClient;

    public PagerDutyStrategy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RunAsync()
    {
        var response = await _httpClient.GetAsync("incidents.json");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
    }
}
