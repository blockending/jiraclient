using System;
using System.Linq;
using JiraClient;
using MetricsClientSample.Strategies;
using GitHubClient;
using PagerDutyClient;
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
        services.AddGitHubClient();
        services.AddPagerDutyClient();
        services.AddTransient<JiraStrategy>();
        services.AddTransient<IApiClientStrategy>(sp =>
        {
            return serviceChoice switch
            {
                "github" => sp.GetRequiredService<GitHubStrategy>(),
                "pagerduty" => sp.GetRequiredService<PagerDutyStrategy>(),
                _ => sp.GetRequiredService<JiraStrategy>()
            };
        });
    });

var host = builder.Build();
var strategy = host.Services.GetRequiredService<IApiClientStrategy>();
await strategy.RunAsync();
