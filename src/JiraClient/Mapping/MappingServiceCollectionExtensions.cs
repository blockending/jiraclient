using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JiraClient.Mapping;

public static class MappingServiceCollectionExtensions
{
    public static IServiceCollection AddDynamicMapping(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<DynamicMappingService>();
        return services;
    }
}
