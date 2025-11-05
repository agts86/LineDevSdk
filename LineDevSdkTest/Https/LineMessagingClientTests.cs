using System.Net;
using LineDevSdk.DTOs.MessagingAPIs;
using LineDevSdk.Https;
using Moq;
using Moq.Protected;

namespace LineDevSdkTest.Https;

public class LineMessagingClientTests
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
        var LineMessagingClient = new LineMessagingClient(httpClient);
        var reply = new Reply();

        // Act
        await LineMessagingClient.PostReplyAsync(reply,"dummyToken");

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(HttpMethod.Post, capturedRequest.Method);
        Assert.Equal("https://api.line.me/v2/bot/message/reply", capturedRequest.RequestUri.ToString());
        Assert.Equal("Bearer", capturedRequest.Headers.Authorization?.Scheme);
        Assert.Equal("dummyToken", capturedRequest.Headers.Authorization?.Parameter);
    }
}

