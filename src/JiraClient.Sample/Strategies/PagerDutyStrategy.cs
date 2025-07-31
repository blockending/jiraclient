using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Linq;
using JiraClient.Mapping;
using JiraClient.Sample;

namespace JiraClient.Sample.Strategies;

public class PagerDutyStrategy : IApiClientStrategy
{
    private readonly HttpClient _httpClient;
    private readonly DynamicMappingService _mapper;

    public PagerDutyStrategy(HttpClient httpClient, DynamicMappingService mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task RunAsync()
    {
        var response = await _httpClient.GetAsync("incidents.json");
        response.EnsureSuccessStatusCode();
        var incidentList = await response.Content.ReadFromJsonAsync<PagerDutyIncidentList>();
        var incident = incidentList?.Incidents.FirstOrDefault();
        if (incident is not null)
        {
            var mapped = _mapper.Map<UnifiedIssue>("pagerduty", incident);
            Console.WriteLine(JsonSerializer.Serialize(mapped, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
