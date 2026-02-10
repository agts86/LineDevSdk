using System.Text.Json.Serialization;
using LineDevSdk.DTOs.Commons.Messages.Templates;
using LineDevSdk.Jsons;

namespace LineDevSdk.DTOs.Commons.Messages;

/// <summary>
/// テンプレートメッセージ
/// </summary>
public class TemplateMessage : IMessage
{
    /// <summary>
    /// タイプ
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; } = "template";

    /// <summary>
    /// テキスト
    /// </summary>
    [JsonPropertyName("altText")]
    public string AltText { get; set; }

    /// <summary>
    /// テンプレート
    /// </summary>
    [JsonConverter(typeof(RealMoldConverter<ITemplate>))]
    [JsonPropertyName("template")]
    public ITemplate Template { get; set; }
}
