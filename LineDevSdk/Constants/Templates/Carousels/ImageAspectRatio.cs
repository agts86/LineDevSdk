using System.Text.Json.Serialization;

namespace LineDevSdk.Constants.Templates.Carousels;

/// <summary>
/// 画像のアスペクト比
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImageAspectRatio
{
    /// <summary>
    /// 1.51:1
    /// </summary>
    rectangle,

    /// <summary>
    /// 1:1
    /// </summary>
    square
}
