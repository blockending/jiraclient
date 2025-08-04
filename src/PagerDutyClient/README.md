# PagerDutyClient

Client for the PagerDuty status API.

## Usage

Add the client to your service collection:

```csharp
var services = new ServiceCollection()
    .AddPagerDutyClient(configuration);
var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IPagerDutyClient>();
```

Retrieve current incidents with `GetIncidentsAsync`:

```csharp
var incidents = await client.GetIncidentsAsync();
```

The HTTP client's base address defaults to `https://status.pagerduty.com/api/v2/` if not set. Providing a `PagerDuty:BaseUrl` configuration value overrides the base address. If a `PagerDuty:ApiKey` is configured, an `Authorization` header is added using the `Token token=` format.

## Metric Profiles

`MetricsClientSample` can retrieve PagerDuty data via metric profiles declared in
`appsettings.*.json`. A profile specifies the client name, endpoints, and
response properties to record.

```json
"MetricProfiles": {
  "pagerduty-acknowledged": {
    "Name": "Acknowledged Alerts",
    "Client": "pagerduty",
    "Endpoints": ["incidents.json?statuses=acknowledged"],
    "Properties": ["incidents.id", "incidents.status", "incidents.summary"]
  }
}
```

Run the sample profile with:

```bash
dotnet run --project src/MetricsClientSample -- pagerduty-acknowledged
```
