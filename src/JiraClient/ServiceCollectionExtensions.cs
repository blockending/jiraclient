using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JiraClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJiraClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JiraOptions>(configuration.GetSection("Jira"));
        services.AddHttpClient<IJiraClient, JiraClientImpl>();
        return services;
    }
}
