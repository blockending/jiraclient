using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraClient.Sample;

public class PagerDutyIncidentList
{
    [JsonPropertyName("incidents")]
    public List<PagerDutyIncident> Incidents { get; set; } = new();
}

public class PagerDutyIncident
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;
}
