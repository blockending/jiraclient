using System;
using System.Linq;
using JiraClient;
using GitHubClient;
using PagerDutyClient;
using MetricsClientSample.Factory;
using JiraClient.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var serviceChoice = args.FirstOrDefault()?.ToLowerInvariant();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.Development.json", optional: true)
              .AddJsonFile("appsettings.Production.json", optional: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddJiraClient(context.Configuration);
        services.AddDynamicMapping(context.Configuration);
        services.AddGitHubClient(context.Configuration);
        services.AddPagerDutyClient(context.Configuration);
        services.AddSingleton<IApiClientFactory, ApiClientFactory>();
    });

var host = builder.Build();
var factory = host.Services.GetRequiredService<IApiClientFactory>();
var mapper = host.Services.GetRequiredService<DynamicMappingService>();

switch (serviceChoice)
{
    case "github":
        var ghClient = (IGitHubClient)factory.CreateClient("github");
        var repo = await ghClient.GetRepoAsync("dotnet", "runtime");
        if (repo is not null)
        {
            var mapped = mapper.Map<UnifiedIssue>("github", repo);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(mapped, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        }
        break;
    case "pagerduty":
        var pdClient = (IPagerDutyClient)factory.CreateClient("pagerduty");
        var incidentList = await pdClient.GetIncidentsAsync();
        var incident = incidentList?.Incidents.FirstOrDefault();
        if (incident is not null)
        {
            var mapped = mapper.Map<UnifiedIssue>("pagerduty", incident);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(mapped, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        }
        break;
    default:
        var jiraClient = (IJiraClient)factory.CreateClient("jira");
        var issue = await jiraClient.GetIssueAsync("TEST-1");
        var mappedIssue = mapper.Map<UnifiedIssue>("jira", issue);
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(mappedIssue, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        break;
}
