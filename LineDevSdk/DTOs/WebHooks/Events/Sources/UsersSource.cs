using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.WebHooks.Events.Sources;

/// <summary>
/// 送信元複数人トーク情報
/// </summary>
public class UsersSource : UserSource
{
    /// <summary>
    /// 送信元複数人トークのトークルームID
    /// </summary>
    [Required]
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    /// <summary>
    /// 送信元ユーザーのID
    /// </summary>
    [JsonPropertyName("userId")]
    public override string UserId { get; set; }
}
