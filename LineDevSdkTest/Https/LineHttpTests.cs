using System.Net;
using LineDevSdk.DTOs.MessagingAPIs;
using LineDevSdk.Https;
using Moq;
using Moq.Protected;

namespace LineDevSdkTest.Https;

public class LineHttpTests
{
    [Fact]
    public async Task PostReplyAsync_SendsCorrectRequest()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage capturedRequest = null;
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, ct) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{}") });
        var httpClient = new HttpClient(handlerMock.Object);
        var lineHttp = new LineHttp(httpClient);
        var reply = new Reply();

        // Act
        await lineHttp.PostReplyAsync(reply,"https://api.line.me/v2/bot/message/replay","dummyToken");

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(HttpMethod.Post, capturedRequest.Method);
        Assert.Equal("https://api.line.me/v2/bot/message/replay", capturedRequest.RequestUri.ToString());
        Assert.Equal("Bearer", capturedRequest.Headers.Authorization?.Scheme);
        Assert.Equal("dummyToken", capturedRequest.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task PostReplyAsync_SendsCorrectRequest_NotHttpClient()
    {
        // Arrange
        var lineHttp = new LineHttpMock();
        var reply = new Reply();

        // Act
        var result = await Record.ExceptionAsync(() =>lineHttp.PostReplyAsync(reply,"https://api.line.me/v2/bot/message/replay","dummyToken"));

        // Assert
        Assert.Null(result);
    }

    public class LineHttpMock : LineHttp
    {
        public override async Task PostReplyAsync(Reply Reply, string endPointUrl, string token)
        {
            await Task.CompletedTask; // Mock implementation
        }
    }
}

