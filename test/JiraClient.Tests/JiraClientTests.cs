using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JiraClient;
using Microsoft.Extensions.Options;
using Xunit;

public class JiraClientTests
{
    private class FakeHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"key\":\"TEST-1\"}")
            };
            return Task.FromResult(response);
        }
    }

    [Fact]
    public async Task GetIssueAsync_ReturnsContent()
    {
        var handler = new FakeHandler();
        var httpClient = new HttpClient(handler);
        var options = Options.Create(new JiraOptions { BaseUrl = "http://localhost" });
        var client = new JiraClientImpl(httpClient, options);

        var result = await client.GetIssueAsync("TEST-1");
        Assert.Contains("TEST-1", result);
    }
}
