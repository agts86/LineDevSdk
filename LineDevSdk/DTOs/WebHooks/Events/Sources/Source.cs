using System.ComponentModel.DataAnnotations;

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
    public string Type { get; set; }

    /// <summary>
    /// 送信元ユーザーのID
    /// </summary>
    public abstract string UserId { get; set; }
}
