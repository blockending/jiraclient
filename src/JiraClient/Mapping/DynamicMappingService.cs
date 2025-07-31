using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace JiraClient.Mapping;

public class DynamicMappingService
{
    private readonly IConfiguration _configuration;

    public DynamicMappingService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TTarget Map<TTarget>(string clientType, object source) where TTarget : new()
    {
        var mapping = _configuration.GetSection($"Mappings:{clientType}").Get<Dictionary<string, string>>() ?? new();
        return DynamicMapper.Map<TTarget>(source, mapping);
    }
}
