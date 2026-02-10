using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTO.Commons.Messages;

/// <summary>
/// テキストメッセージ
/// </summary>
public class TextMessage : IMessage
{
    /// <summary>
    /// タイプ
    /// </summary>
    [Required]
    [JsonPropertyName("type")]
    public string Type { get; } = "text";

    /// <summary>
    /// テキスト
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
