using System;
using System.Text.Json;
using JiraClient;
using JiraClient.Mapping;

namespace JiraClient.Sample.Strategies;

public class JiraStrategy : IApiClientStrategy
{
    private readonly IJiraClient _client;
    private readonly DynamicMappingService _mapper;

    public JiraStrategy(IJiraClient client, DynamicMappingService mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task RunAsync()
    {
        var issue = await _client.GetIssueAsync("TEST-1");
        var mapped = _mapper.Map<UnifiedIssue>("jira", issue);
        Console.WriteLine(JsonSerializer.Serialize(mapped, new JsonSerializerOptions { WriteIndented = true }));
    }
}
