using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LineDevSdk.DTOs.Commons.Messages.Actions;

public class UriAction : IAction
{
    [JsonPropertyName("type")]
    public string Type { get; } = "uri";

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [Required]
    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}
