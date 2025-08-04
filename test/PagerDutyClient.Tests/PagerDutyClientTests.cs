using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PagerDutyClient;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

public class PagerDutyClientTests
{
    [Fact]
    public async Task GetIncidentsAsync_ReturnsIncidents()
    {
        // arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{\"incidents\":[{\"id\":\"1\",\"status\":\"triggered\",\"summary\":\"Test\"}]}")
                   });
        var httpClient = new HttpClient(handlerMock.Object);
        var client = new PagerDutyClientImpl(httpClient, Options.Create(new PagerDutyOptions()));

        // act
        var list = await client.GetIncidentsAsync();

        // assert
        Assert.Single(list!.Incidents);
    }

    [Fact]
    public void Constructor_SetsDefaultBaseAddress()
    {
        // arrange
        var httpClient = new HttpClient(new HttpClientHandler());

        // act
        var client = new PagerDutyClientImpl(httpClient, Options.Create(new PagerDutyOptions()));

        // assert
        Assert.Equal("https://status.pagerduty.com/api/v2/", httpClient.BaseAddress!.ToString());
    }

    private class RecordingHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") });
        }
    }

    [Fact]
    public async Task AuthorizationHeader_IsAdded_WhenApiKeyProvided()
    {
        var recordingHandler = new RecordingHandler();
        var options = Options.Create(new PagerDutyOptions { ApiKey = "xyz" });
        var apiHandler = new ApiKeyHttpMessageHandler(options)
        {
            InnerHandler = recordingHandler
        };
        var httpClient = new HttpClient(apiHandler);
        var client = new PagerDutyClientImpl(httpClient, options);

        await client.GetIncidentsAsync();

        Assert.Equal("Token token=xyz", recordingHandler.LastRequest!.Headers.Authorization!.ToString());
    }

    [Fact]
    public void Constructor_UsesBaseUrlFromOptions()
    {
        var httpClient = new HttpClient(new HttpClientHandler());
        var options = Options.Create(new PagerDutyOptions { BaseUrl = "https://example.com/" });
        var client = new PagerDutyClientImpl(httpClient, options);

        Assert.Equal("https://example.com/", httpClient.BaseAddress!.ToString());
    }
}
