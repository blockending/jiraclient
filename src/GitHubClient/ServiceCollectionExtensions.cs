using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitHubClient(this IServiceCollection services, IConfiguration? configuration = null)
    {
        if (configuration != null)
        {
            services.Configure<GitHubOptions>(configuration.GetSection("GitHub"));
        }
        services.AddTransient<PatHttpMessageHandler>();
        services.AddHttpClient<IGitHubClient, GitHubClientImpl>()
                .AddHttpMessageHandler<PatHttpMessageHandler>();
        return services;
    }
}
