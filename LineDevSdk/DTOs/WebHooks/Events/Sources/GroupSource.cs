using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.WebHooks.Events.Sources;

/// <summary>
/// 送信元グループ情報
/// </summary>
public class GroupSource : UserSource
{
    /// <summary>
    /// 送信元グループのID
    /// </summary>
    [Required]
    [JsonPropertyName("groupId")]
    public string GroupId { get; set; }

    /// <summary>
    /// 送信元ユーザーのID
    /// </summary>
    [JsonPropertyName("userId")]
    public override string UserId { get; set; }
}
