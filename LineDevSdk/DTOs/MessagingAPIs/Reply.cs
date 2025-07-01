using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LineDevSdk.Jsons;
using LineDevSdk.DTOs.Commons.Messages;

namespace LineDevSdk.DTOs.MessagingAPIs;

/// <summary>
/// 応答メッセージ
/// </summary>
public class Reply
{
    /// <summary>
    /// 応答トークン
    /// </summary>
    [Required]
    [JsonPropertyName("replyToken")]
    public string ReplyToken {get; set;}

    /// <summary>
    /// メッセージ
    /// </summary>
    [JsonConverter(typeof(RealMoldConverter<IMessage[]>))]
    [JsonPropertyName("messages")]
    public IMessage[] Messages {get; set;}
}
