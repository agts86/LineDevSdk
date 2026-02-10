using System.Text.Json.Serialization;
using LineDevSdk.DTO.Commons.Messages.Templates;
using LineDevSdk.Converter;

namespace LineDevSdk.DTO.Commons.Messages;

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
