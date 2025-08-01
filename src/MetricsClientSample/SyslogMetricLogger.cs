using System;

namespace MetricsClientSample;

public interface IMetricLogger
{
    void Log(string metricName, string rawData);
}

public class SyslogMetricLogger : IMetricLogger
{
    public void Log(string metricName, string rawData)
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK");
        var host = Environment.MachineName;
        Console.WriteLine($"{timestamp} {host} MetricsClientSample: {metricName} {rawData}");
    }
}
