using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JiraClient;
using Microsoft.Extensions.Options;
using Xunit;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Moq.Protected;

public class JiraClientTests
{

    private class RecordingHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}")
            };
            return Task.FromResult(response);
        }
    }

    private class FakeTokenProvider : IOAuthTokenProvider
    {
        public Task<string> GetAccessTokenAsync() => Task.FromResult("abc123");
    }

    [Fact]
    public async Task GetIssueAsync_ReturnsContent()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        var handlerMock = fixture.Create<Mock<HttpMessageHandler>>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{\"key\":\"TEST-1\"}")
                   });

        var httpClient = new HttpClient(handlerMock.Object);
        var options = Options.Create(new JiraOptions { BaseUrl = "http://localhost" });
        var client = new JiraClientImpl(httpClient, options);

        var result = await client.GetIssueAsync("TEST-1");
        Assert.Equal("TEST-1", result.Key);
    }

    [Fact]
    public async Task AuthorizationHeader_IsAdded()
    {
        var recordingHandler = new RecordingHandler();
        var authHandler = new OAuthHttpMessageHandler(new FakeTokenProvider())
        {
            InnerHandler = recordingHandler
        };
        var httpClient = new HttpClient(authHandler);
        var options = Options.Create(new JiraOptions { BaseUrl = "http://localhost" });
        var client = new JiraClientImpl(httpClient, options);

        await client.GetIssueAsync("TEST-1");

        Assert.Equal("Bearer abc123", recordingHandler.LastRequest!.Headers.Authorization!.ToString());
    }
}
