namespace PagerDutyClient;

public class PagerDutyOptions
{
    public string BaseUrl { get; set; } = "https://status.pagerduty.com/api/v2/";
    public string ApiKey { get; set; } = string.Empty;
}
