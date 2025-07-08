using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.WebHooks.Events.Sources;

/// <summary>
/// 送信元ユーザー情報
/// </summary>
public class UserSource : Source
{
    /// <summary>
    /// 送信元ユーザーのID
    /// </summary>
    [Required]
    [JsonPropertyName("userId")]
    public override string UserId { get; set; }
}
