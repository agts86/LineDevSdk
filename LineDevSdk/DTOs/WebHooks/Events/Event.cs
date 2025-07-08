using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LineDevSdk.DTOs.WebHooks.Events.Sources;

namespace LineDevSdk.DTOs.WebHooks.Events;

/// <summary>
/// Webhookイベントオブジェクト
/// </summary>
public abstract class Event
{
    /// <summary>
    /// イベントのタイプ
    /// </summary>
    public abstract string Type {get;}

    /// <summary>
    /// チャネルの状態
    /// </summary>
    [Required]
    [JsonPropertyName("mode")]
    public string Mode {get; set;}

    /// <summary>
    /// イベントの発生時刻
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long Timestamp {get; set;}

    /// <summary>
    /// イベントの送信元情報を含むオブジェクト
    /// </summary>
    [JsonPropertyName("source")]
    public UserSource Source {get; set;}

    /// <summary>
    /// WebhookイベントID
    /// </summary>
    [Required]
    [JsonPropertyName("webhookEventId")]
    public string WebhookEventId {get; set;}

    /// <summary>
    /// Webhookイベントが再送されたものかどうか
    /// </summary>
    [JsonPropertyName("deliveryContext")]
    public DeliveryContextInfo DeliveryContext {get; set;}

    /// <summary>
    /// Webhookイベントが再送されたものかどうか
    /// </summary>
    public class DeliveryContextInfo
    {
        /// <summary>
        /// Webhookイベントが再送されたものかどうか
        /// </summary>
        [JsonPropertyName("isRedelivery")]
        public bool IsRedelivery {get; set;}
    }
}
