using System.Text.Json;
using LineDevSdk.DTO.Commons.Messages;
using LineDevSdk.Converter;

namespace LineDevSdkTest.Converter;

public class MessageConverterTests
{
    [Fact]
    public void Read_DeserializePolymorphicMessage_Success()
    {
        // Arrange: TextMessage
        var json = "{\"type\":\"text\",\"text\":\"hello\"}";
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new MessageConverter());

        // Act
        var result = JsonSerializer.Deserialize<IMessage>(json, options);

        // Assert
        var textMsg = Assert.IsType<TextMessage>(result);
        Assert.Equal("text", textMsg.Type);
        Assert.Equal("hello", textMsg.Text);
    }

    [Fact]
    public void Write_SerializePolymorphicMessage_Success()
    {
        // Arrange
        IMessage msg = new TextMessage { Text = "hi" };
        var options = new JsonSerializerOptions { WriteIndented = false, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new MessageConverter());

        // Act
        var json = JsonSerializer.Serialize(msg, options);

        // Assert
        Assert.Contains("\"type\":\"text\"", json);
        Assert.Contains("\"text\":\"hi\"", json);
    }
}

