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

## Running the Sample

1. **Start the mock Jira service** using [Mountebank](http://www.mbtest.org/):
   ```bash
   cd mountebank
   ./start-mountebank.sh
   ```
2. **Run the console app** from Visual Studio or Rider:
   - Open the solution `JiraClient.sln`.
   - Set `JiraClient.Sample` as the startup project.
   - Run the application. It will fetch the issue `TEST-1` from the mock service and print the JSON.

## Azure DevOps Build

An example Azure pipeline is provided in `azure-pipelines.yml`. It restores, builds and runs the unit tests using the .NET 8 SDK.

