using System.Text.Json.Serialization;
using LineDevSdk.DTO.WebHooks.Events;
using LineDevSdk.Converter;

namespace LineDevSdk.DTO.WebHooks;

/// <summary>
/// PostGourmetLocationAsyncのリクエストDto
/// </summary>
public class WebHook
{
    /// <summary>
    /// Webhookイベントを受信すべきボットのユーザーID
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination {get; set;}

    /// <summary>
    /// Webhookイベントオブジェクトの配列
    /// </summary>
    [JsonPropertyName("events")]
    [JsonConverter(typeof(EventConverter))]
    public Event[] Events { get; set; } = [];
}
