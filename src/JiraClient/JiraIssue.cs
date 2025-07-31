using System.Text.Json.Serialization;

namespace JiraClient;

public class JiraIssue
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("self")]
    public string Self { get; set; } = string.Empty;

    [JsonPropertyName("fields")]
    public IssueFields Fields { get; set; } = new IssueFields();
}

public class IssueFields
{
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("issuetype")]
    public IssueType? IssueType { get; set; }

    [JsonPropertyName("project")]
    public ProjectInfo? Project { get; set; }

    [JsonPropertyName("status")]
    public StatusInfo? Status { get; set; }
}

public class IssueType
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class ProjectInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class StatusInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
