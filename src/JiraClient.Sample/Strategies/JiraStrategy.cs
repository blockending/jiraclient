using System;
using System.Text.Json;
using JiraClient;

namespace JiraClient.Sample.Strategies;

public class JiraStrategy : IApiClientStrategy
{
    private readonly IJiraClient _client;

    public JiraStrategy(IJiraClient client)
    {
        _client = client;
    }

    public async Task RunAsync()
    {
        var issue = await _client.GetIssueAsync("TEST-1");
        Console.WriteLine(JsonSerializer.Serialize(issue, new JsonSerializerOptions { WriteIndented = true }));
    }
}
