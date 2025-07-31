using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JiraClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJiraClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JiraOptions>(configuration.GetSection("Jira"));
        services.Configure<OAuthOptions>(configuration.GetSection("Jira:OAuth"));
        services.AddHttpClient<IOAuthTokenProvider, OAuthTokenProvider>();
        services.AddTransient<OAuthHttpMessageHandler>();
        services.AddHttpClient<IJiraClient, JiraClientImpl>()
                .AddHttpMessageHandler<OAuthHttpMessageHandler>();
        return services;
    }
}
