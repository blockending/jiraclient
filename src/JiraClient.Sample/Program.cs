using JiraClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.Development.json", optional: true)
              .AddJsonFile("appsettings.Production.json", optional: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddJiraClient(context.Configuration);
    });

var host = builder.Build();
var client = host.Services.GetRequiredService<IJiraClient>();

var issue = await client.GetIssueAsync("TEST-1");
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(issue, new System.Text.Json.JsonSerializerOptions
{
    WriteIndented = true
}));
