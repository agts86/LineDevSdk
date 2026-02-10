using System.Text.Json.Serialization;

namespace LineDevSdk.DTO.Commons.Messages;

/// <summary>
/// テキストV2メッセージ
/// </summary>
public class TextV2Message : IMessage
{
    /// <summary>
    /// タイプ
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; } = "textV2";

    /// <summary>
    /// テキスト
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
