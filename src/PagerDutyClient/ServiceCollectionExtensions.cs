using Microsoft.Extensions.DependencyInjection;

namespace PagerDutyClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPagerDutyClient(this IServiceCollection services)
    {
        services.AddHttpClient<IPagerDutyClient, PagerDutyClientImpl>();
        return services;
    }
}
