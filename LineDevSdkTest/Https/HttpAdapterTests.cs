using System.Net;
using System.Text.Json;
using LineDevSdk.Https;
using Moq;
using Moq.Protected;

namespace LineDevSdkTest.Https;

public class HttpAdapterTests
{
    [Fact]
    public async Task GetAsync_ReturnsDeserializedObject()
    {
        var expected = new TestDto { Name = "test", Value = 123 };
        var json = JsonSerializer.Serialize(expected);
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        var result = await adapter.GetAsync<TestDto>("https://dummy");

        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Value, result.Value);
    }

    [Fact]
    public async Task PostAsync_ReturnsDeserializedObject()
    {
        var expected = new TestDto { Name = "post", Value = 456 };
        var json = JsonSerializer.Serialize(expected);
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.Content.ReadAsStringAsync().Result.Contains("post")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        var result = await adapter.PostAsync<TestDto, TestDto>("https://dummy", expected);

        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Value, result.Value);
    }

    [Fact]
    public async Task PutAsync_ReturnsDeserializedObject()
    {
        var expected = new TestDto { Name = "put", Value = 789 };
        var json = JsonSerializer.Serialize(expected);
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.Content.ReadAsStringAsync().Result.Contains("put")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        var result = await adapter.PutAsync<TestDto, TestDto>("https://dummy", expected);

        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Value, result.Value);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsDeserializedObject()
    {
        var expected = new TestDto { Name = "delete", Value = 999 };
        var json = JsonSerializer.Serialize(expected);
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.Content.ReadAsStringAsync().Result.Contains("delete")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        var result = await adapter.DeleteAsync<TestDto, TestDto>("https://dummy", expected);

        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Value, result.Value);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_OnErrorResponse()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("error")
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        await Assert.ThrowsAsync<HttpRequestException>(() => adapter.GetAsync<TestDto>("https://dummy"));
    }

    [Fact]
    public async Task PostAsync_ThrowsException_OnErrorResponse()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("error")
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        await Assert.ThrowsAsync<HttpRequestException>(() => adapter.PostAsync<TestDto, TestDto>("https://dummy", new TestDto()));
    }

    [Fact]
    public async Task GetAsync_ThrowsException_OnErrorResponse_MessageContainsStatusAndBody()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("error details")
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var adapter = new HttpAdapter(httpClient);

        var ex = await Assert.ThrowsAsync<HttpRequestException>(() => adapter.GetAsync<TestDto>("https://dummy"));
        Assert.Contains("400 BadRequest", ex.Message);
        Assert.Contains("error details", ex.Message);
    }

    public class TestDto
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}

