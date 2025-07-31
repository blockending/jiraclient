using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PagerDutyClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPagerDutyClient(this IServiceCollection services, IConfiguration? configuration = null)
    {
        if (configuration != null)
        {
            services.Configure<PagerDutyOptions>(configuration.GetSection("PagerDuty"));
        }
        services.AddTransient<ApiKeyHttpMessageHandler>();
        services.AddHttpClient<IPagerDutyClient, PagerDutyClientImpl>()
                .AddHttpMessageHandler<ApiKeyHttpMessageHandler>();
        return services;
    }
}
