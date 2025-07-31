using System;
using System.Linq;
using JiraClient;
using JiraClient.Sample.Strategies;
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
        services.AddHttpClient<GitHubStrategy>(c =>
        {
            c.BaseAddress = new Uri("https://api.github.com/");
            c.DefaultRequestHeaders.UserAgent.ParseAdd("jiraclient-sample");
        });
        services.AddHttpClient<PagerDutyStrategy>(c =>
        {
            c.BaseAddress = new Uri("https://status.pagerduty.com/api/v2/");
        });
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
