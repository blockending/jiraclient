# JiraClient

This repository contains a small HTTP client library for Jira along with a sample console application. The client follows SOLID design principles and exposes an `IJiraClient` interface for consumers.

## Projects

- **JiraClient** – .NET class library that wraps minimal Jira REST API calls.
- **JiraClient.Sample** – console application demonstrating usage of the client.
- **JiraClient.Tests** – xUnit test project.

## Jira API Reference

The Jira REST API is documented at [Atlassian's developer site](https://developer.atlassian.com/cloud/jira/platform/rest/v2/intro/). This client only implements the `/rest/api/2/issue/{issueKey}` endpoint as an example but can be extended.

## Configuration

Environment specific configuration files are used:

- `appsettings.Development.json` – points the client at the local Mountebank mock (`http://localhost:4545`).
- `appsettings.Production.json` – example production URL placeholder.

Both files expose a `Jira:BaseUrl` setting that the library reads via dependency injection.

## Configuring Authentication

Authentication is performed using OAuth. Provide the OAuth settings under
`Jira:OAuth` in your configuration file or as environment variables. A typical
configuration section looks like:

```json
"Jira": {
  "BaseUrl": "https://your.jira.server",
  "OAuth": {
    "TokenUrl": "https://your.jira.server/oauth/token",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RefreshToken": "your-refresh-token"
  }
}
```

`Host.CreateDefaultBuilder` reads environment variables as well, so values such
as `Jira__OAuth__ClientId` or `Jira__OAuth__RefreshToken` can be supplied at
runtime without modifying the JSON files.

## Running the Sample

1. **Start the mock Jira service** using [Mountebank](http://www.mbtest.org/):
   ```bash
   cd mountebank
   ./start-mountebank.sh
   ```
   The imposter requires an `Authorization` header bearing a token. It also
   determines the HTTP status code from the requested issue key. Requesting
   `TEST-200` yields a `200` response, `TEST-404` returns `404`, and so on. The
   predefined issue `TEST-1` continues to return `200`.
2. **Run the console app** from Visual Studio or Rider:
   - Open the solution `JiraClient.sln`.
   - Set `JiraClient.Sample` as the startup project.
   - Run the application. It will fetch the issue `TEST-1` from the mock service and print the JSON.

## Azure DevOps Build

An example Azure pipeline is provided in `azure-pipelines.yml`. It restores, builds and runs the unit tests using the .NET 8 SDK.

