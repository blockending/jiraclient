using System;
using PagerDutyClient;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using JiraClient.Mapping;
using MetricsClientSample;

namespace MetricsClientSample.Strategies;

public class PagerDutyStrategy : IApiClientStrategy
{
    private readonly IPagerDutyClient _client;
    private readonly DynamicMappingService _mapper;

    public PagerDutyStrategy(IPagerDutyClient client, DynamicMappingService mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task RunAsync()
    {
        var incidentList = await _client.GetIncidentsAsync();
        var incident = incidentList?.Incidents.FirstOrDefault();
        if (incident is not null)
        {
            var mapped = _mapper.Map<UnifiedIssue>("pagerduty", incident);
            Console.WriteLine(JsonSerializer.Serialize(mapped, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
