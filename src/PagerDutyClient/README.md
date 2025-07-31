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

The HTTP client's base address defaults to `https://status.pagerduty.com/api/v2/` if not set. If a `PagerDuty:ApiKey` is configured, an `Authorization` header is added using the `Token token=` format.
