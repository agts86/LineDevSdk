using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LineDevSdk.DTO.Commons.Messages;
using LineDevSdk.Converter;

namespace LineDevSdk.DTO.WebHooks.Events;

public class MessageEvent : Event
{
    [Required]
    public override string Type { get; } = "message";

    /// <summary>
    /// 応答トークン
    /// </summary>
    [Required]
    [JsonPropertyName("replyToken")]
    public string ReplyToken {get; set;}

    /// <summary>
    /// メッセージの内容を含むオブジェクト
    /// </summary>
    [Required]
    [JsonConverter(typeof(MessageConverter))]
    public IMessage Message {get; set;}
}
