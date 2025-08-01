# metricsaggregator

This repository hosts several small .NET HTTP client libraries and a sample console application. Each client targets a different service and is designed following SOLID principles with interfaces that can be easily consumed or mocked in other projects.

## Projects

The solution is composed of several libraries and associated test projects:

- **JiraClient** – minimal wrapper around the Jira REST API.
- **GitHubClient** – tiny client for the GitHub REST API.
- **PagerDutyClient** – simple client for the PagerDuty status API.
- **MetricsClientSample** – console application demonstrating the clients and the dynamic mapping service.
- **JiraClient.Tests**, **GitHubClient.Tests**, **PagerDutyClient.Tests** – xUnit projects covering the libraries.

## Jira API Reference

The Jira REST API is documented at [Atlassian's developer site](https://developer.atlassian.com/cloud/jira/platform/rest/v2/intro/). This client only implements the `/rest/api/2/issue/{issueKey}` endpoint as an example but can be extended.

## Configuration

The sample console app uses environment specific configuration files:

- `appsettings.Development.json` – points the Jira client at the local mock service (`http://localhost:4545`).
- `appsettings.Production.json` – example production values.

Each file contains a `Jira` section with the base URL and OAuth settings and a
`Mappings` section used by `DynamicMappingService` to translate the raw API
models into a unified shape. Values can also be supplied via environment
variables when running the application.

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

For GitHub and PagerDuty, supply `GitHub:PersonalAccessToken` and `PagerDuty:ApiKey`
in the configuration (or as environment variables `GitHub__PersonalAccessToken`
and `PagerDuty__ApiKey`) to authenticate requests.

## Service Mocks

The folder `mountebank` contains an `imposters.json` file describing HTTP mocks
for the three services and a helper script `start-mountebank.sh` that launches
[Mountebank](http://www.mbtest.org/) with those definitions. The mocks run on
the following ports:

- `4545` – Jira API
- `4546` – GitHub API
- `4547` – PagerDuty API

The Jira imposter inspects the requested issue key to determine the HTTP status
code (e.g. requesting `TEST-404` returns a `404` response).

## Running the Sample

1. **Start the mock services** using [Mountebank](http://www.mbtest.org/):
   ```bash
   cd mountebank
   ./start-mountebank.sh
   ```
   This launches local mocks for Jira (port `4545`), GitHub (`4546`) and
   PagerDuty (`4547`). The Jira imposter requires an `Authorization` header
   bearing a token and determines the HTTP status code from the requested issue
   key. Requesting `TEST-200` yields a `200` response, `TEST-404` returns `404`,
   and so on. The predefined issue `TEST-1` continues to return `200`.
2. **Run the console app** from Visual Studio or Rider:
   - Open the solution `JiraClient.sln`.
   - Set `MetricsClientSample` as the startup project.
   - Run the application. It will fetch the issue `TEST-1` from the mock service and print the JSON.

## Azure DevOps Build

An example Azure pipeline is provided in `azure-pipelines.yml`. It restores, builds and runs the unit tests using the .NET 8 SDK.


## License

This project is licensed under the MIT No Attribution (MIT-0) license. See the [LICENSE](LICENSE) file for details.
