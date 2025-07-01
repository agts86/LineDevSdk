using System.ComponentModel.DataAnnotations;

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
    public string Type { get; } = "location";

    /// <summary>
    /// タイトル
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 住所
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 緯度
    /// </summary>
    public double Latitude {get; set;}

    /// <summary>
    /// 軽度
    /// </summary>
    public double Longitude {get; set;}
}
