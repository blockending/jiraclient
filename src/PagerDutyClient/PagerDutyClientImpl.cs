using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PagerDutyClient;

public class PagerDutyClientImpl : IPagerDutyClient
{
    private readonly HttpClient _httpClient;

    public PagerDutyClientImpl(HttpClient httpClient)
    {
        _httpClient = httpClient;
        if (_httpClient.BaseAddress == null)
            _httpClient.BaseAddress = new System.Uri("https://status.pagerduty.com/api/v2/");
    }

    public async Task<PagerDutyIncidentList?> GetIncidentsAsync()
    {
        var response = await _httpClient.GetAsync("incidents.json");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PagerDutyIncidentList>();
    }

    public async Task<string> GetRawAsync(string path)
    {
        var response = await _httpClient.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
