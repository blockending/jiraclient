using System;
using GitHubClient;
using JiraClient;
using PagerDutyClient;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsClientSample.Factory;

public interface IApiClientFactory
{
    object CreateClient(string clientType);
}

public class ApiClientFactory : IApiClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ApiClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object CreateClient(string clientType)
    {
        return clientType.ToLowerInvariant() switch
        {
            "github" => _serviceProvider.GetRequiredService<IGitHubClient>(),
            "pagerduty" => _serviceProvider.GetRequiredService<IPagerDutyClient>(),
            "jira" => _serviceProvider.GetRequiredService<IJiraClient>(),
            _ => throw new ArgumentException($"Unknown client type '{clientType}'", nameof(clientType))
        };
    }
}
