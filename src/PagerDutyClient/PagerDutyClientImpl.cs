using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PagerDutyClient;

public class PagerDutyClientImpl : IPagerDutyClient
{
    private readonly HttpClient _httpClient;
    private readonly PagerDutyOptions _options;

    public PagerDutyClientImpl(HttpClient httpClient, IOptions<PagerDutyOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        if (_httpClient.BaseAddress == null)
            _httpClient.BaseAddress = new System.Uri(_options.BaseUrl);
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
