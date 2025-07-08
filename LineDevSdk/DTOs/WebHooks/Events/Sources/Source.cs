using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.WebHooks.Events.Sources;

/// <summary>
/// ソース基底クラス
/// </summary>
public abstract class Source
{
    /// <summary>
    /// タイプ
    /// </summary>
    [Required]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 送信元ユーザーのID
    /// </summary>
    [JsonPropertyName("userId")]
    public abstract string UserId { get; set; }
}
