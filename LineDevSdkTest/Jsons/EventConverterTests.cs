using System.Text.Json;
using LineDevSdk.DTOs.WebHooks.Events;
using LineDevSdk.DTOs.Commons.Messages;
using LineDevSdk.Jsons;

namespace LineDevSdkTest.Jsons;

public class EventConverterTests
{
    [Fact]
    public void Read_DeserializePolymorphicEventArray_Success()
    {
        // Arrange: MessageEvent + TextMessage
        var json = @"[
            {
                ""type"": ""message"",
                ""mode"": ""active"",
                ""timestamp"": 1234567890,
                ""source"": null,
                ""webhookEventId"": ""abc"",
                ""deliveryContext"": { ""isRedelivery"": false },
                ""replyToken"": ""token"",
                ""message"": {
                    ""type"": ""text"",
                    ""text"": ""hello""
                }
            }
        ]";
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new EventConverter());
        options.Converters.Add(new MessageConverter());

        // Act
        var result = JsonSerializer.Deserialize<Event[]>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var msgEvent = Assert.IsType<MessageEvent>(result[0]);
        Assert.Equal("message", msgEvent.Type);
        Assert.Equal("token", msgEvent.ReplyToken);
        Assert.NotNull(msgEvent.Message);
        var textMsg = Assert.IsType<TextMessage>(msgEvent.Message);
        Assert.Equal("text", textMsg.Type);
        Assert.Equal("hello", textMsg.Text);
    }

    [Fact]
    public void Write_SerializePolymorphicEventArray_Success()
    {
        // Arrange
        var events = new Event[]
        {
            new MessageEvent
            {
                Mode = "active",
                Timestamp = 123,
                WebhookEventId = "id",
                ReplyToken = "token",
                Message = new TextMessage { Text = "hi" }
            }
        };
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new EventConverter());
        options.Converters.Add(new MessageConverter());

        // Act
        var json = JsonSerializer.Serialize(events, options);

        // Assert
        Assert.Contains("\"type\":\"message\"", json);
        Assert.Contains("\"type\":\"text\"", json);
        Assert.Contains("\"text\":\"hi\"", json);
    }
}

