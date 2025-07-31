using System;
using GitHubClient;
using System.Text.Json;
using System.Threading.Tasks;
using JiraClient.Mapping;
using MetricsClientSample;

namespace MetricsClientSample.Strategies;

public class GitHubStrategy : IApiClientStrategy
{
    private readonly IGitHubClient _client;
    private readonly DynamicMappingService _mapper;

    public GitHubStrategy(IGitHubClient client, DynamicMappingService mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task RunAsync()
    {
        var repo = await _client.GetRepoAsync("dotnet", "runtime");
        if (repo is not null)
        {
            var mapped = _mapper.Map<UnifiedIssue>("github", repo);
            Console.WriteLine(JsonSerializer.Serialize(mapped, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
