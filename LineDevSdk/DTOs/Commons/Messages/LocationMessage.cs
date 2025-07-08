using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.Commons.Messages;

/// <summary>
/// 位置情報メッセージ
/// </summary>
public class LocationMessage : IMessage
{
    /// <summary>
    /// タイプ
    /// </summary>
    [Required]
    [JsonPropertyName("type")]
    public string Type { get; } = "location";

    /// <summary>
    /// タイトル
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// 住所
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; }

    /// <summary>
    /// 緯度
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude {get; set;}

    /// <summary>
    /// 経度
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude {get; set;}
}
