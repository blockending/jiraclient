using Microsoft.Extensions.DependencyInjection;

namespace GitHubClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitHubClient(this IServiceCollection services)
    {
        services.AddHttpClient<IGitHubClient, GitHubClientImpl>();
        return services;
    }
}
