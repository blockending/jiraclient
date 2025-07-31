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

The client defaults the `BaseAddress` to `https://api.github.com/` and adds a `User-Agent` header if one is not provided.  If a `GitHub:PersonalAccessToken` value is supplied in configuration, an `Authorization` header is automatically added to each request.
