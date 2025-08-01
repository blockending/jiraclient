namespace MetricsClientSample;

public class MetricProfile
{
    public string Name { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public string[] Endpoints { get; set; } = System.Array.Empty<string>();
    public string[] Properties { get; set; } = System.Array.Empty<string>();
}
