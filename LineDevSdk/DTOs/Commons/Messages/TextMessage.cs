using System.ComponentModel.DataAnnotations;

namespace LineDevSdk.DTOs.Commons.Messages;

/// <summary>
/// テキストメッセージ
/// </summary>
public class TextMessage : IMessage
{
    /// <summary>
    /// タイプ
    /// </summary>
    [Required]
    public string Type { get; } = "text";

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text { get; set; }
}
