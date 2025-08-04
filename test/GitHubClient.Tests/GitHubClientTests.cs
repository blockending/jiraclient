using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitHubClient;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

public class GitHubClientTests
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

    [Fact]
    public async Task GetRepoAsync_ReturnsRepo()
    {
        // arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{\"id\":1,\"name\":\"repo\",\"full_name\":\"octocat/repo\",\"visibility\":\"public\"}")
                   });
        var httpClient = new HttpClient(handlerMock.Object);
        var client = new GitHubClientImpl(httpClient, Options.Create(new GitHubOptions()));

        // act
        var repo = await client.GetRepoAsync("octocat", "repo");

        // assert
        Assert.Equal("repo", repo!.Name);
    }

    [Fact]
    public async Task UserAgent_IsAdded()
    {
        // arrange
        var recordingHandler = new RecordingHandler();
        var httpClient = new HttpClient(recordingHandler);
        var client = new GitHubClientImpl(httpClient, Options.Create(new GitHubOptions()));

        // act
        await client.GetRepoAsync("octocat", "repo");

        // assert
        Assert.Contains("metricsclientsample", recordingHandler.LastRequest!.Headers.UserAgent.ToString());
    }

    [Fact]
    public async Task AuthorizationHeader_IsAdded_WhenPatProvided()
    {
        var recordingHandler = new RecordingHandler();
        var options = Options.Create(new GitHubOptions { PersonalAccessToken = "abc" });
        var patHandler = new PatHttpMessageHandler(options)
        {
            InnerHandler = recordingHandler
        };
        var httpClient = new HttpClient(patHandler);
        var client = new GitHubClientImpl(httpClient, options);

        await client.GetRepoAsync("octocat", "repo");

        Assert.Equal("token abc", recordingHandler.LastRequest!.Headers.Authorization!.ToString());
    }

    [Fact]
    public void Constructor_SetsBaseAddressFromOptions()
    {
        var httpClient = new HttpClient(new HttpClientHandler());
        var options = Options.Create(new GitHubOptions { BaseUrl = "https://example.com/" });
        var client = new GitHubClientImpl(httpClient, options);

        Assert.Equal("https://example.com/", httpClient.BaseAddress!.ToString());
    }
}
