using System;
using System.Linq;
using JiraClient;
using GitHubClient;
using PagerDutyClient;
using MetricsClientSample.Factory;
using MetricsClientSample;
using JiraClient.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var metricChoice = args.FirstOrDefault()?.ToLowerInvariant();

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
        services.AddSingleton<IMetricLogger, SyslogMetricLogger>();
    });

var host = builder.Build();
var factory = host.Services.GetRequiredService<IApiClientFactory>();
var logger = host.Services.GetRequiredService<IMetricLogger>();
var config = host.Services.GetRequiredService<IConfiguration>();

if (string.IsNullOrWhiteSpace(metricChoice))
{
    Console.WriteLine("Please provide a metric profile name.");
    return;
}

var profile = config.GetSection($"MetricProfiles:{metricChoice}").Get<MetricProfile>();
if (profile is null)
{
    Console.WriteLine($"Metric profile '{metricChoice}' not found.");
    return;
}

var client = (dynamic)factory.CreateClient(profile.Client);
foreach (var endpoint in profile.Endpoints)
{
    var raw = await client.GetRawAsync(endpoint);
    logger.Log(metricChoice, raw);
}
