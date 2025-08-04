# GitHubClient

A minimal wrapper around the GitHub REST API.

## Usage

Register the client with dependency injection:

```csharp
var services = new ServiceCollection()
    .AddGitHubClient(configuration);
var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IGitHubClient>();
```

Fetch repository information with `GetRepoAsync`:

```csharp
var repo = await client.GetRepoAsync("octocat", "Hello-World");
```

The client defaults the `BaseAddress` to `https://api.github.com/` and adds a `User-Agent` header if one is not provided. Setting a `GitHub:BaseUrl` configuration value overrides the base address. If a `GitHub:PersonalAccessToken` value is supplied, an `Authorization` header is automatically added to each request.

## Metric Profiles

The `MetricsClientSample` project can drive this client using metric profiles
defined in the `MetricProfiles` section of `appsettings.*.json`.
Each profile identifies the client to use, the endpoints to call, and the
JSON properties to log.

```json
"MetricProfiles": {
  "github-branch-count": {
    "Name": "Number of Branches",
    "Client": "github",
    "Endpoints": ["repos/dotnet/runtime/branches"],
    "Properties": ["name"]
  }
}
```

Run the sample with:

```bash
dotnet run --project src/MetricsClientSample -- github-branch-count
```
