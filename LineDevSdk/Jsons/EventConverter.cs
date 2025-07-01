using System.Text.Json;
using System.Text.Json.Serialization;
using LineDevSdk.DTOs.WebHooks.Events;
using LineDevSdk.Utilities;

namespace LineDevSdk.Jsons;

/// <summary>
/// インターフェイス型プロパティを実際の型に変換するJsonConverter
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventConverter : JsonConverter<Event[]>
{
    public override Event[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var backup = reader;
        using var doc = JsonDocument.ParseValue(ref reader);
        var events = doc.RootElement.EnumerateArray()
            .Select(x =>
            {
                var typeValue = x.EnumerateObject()
                    .Where(y => y.Name.Equals("type", StringComparison.OrdinalIgnoreCase))
                    .Select(y => y.Value.GetString())
                    .SingleOrDefault();
                var convertType = Polymorphism.CreatePolymorphismArray<Event>()
                    .Where(y => y.Type == typeValue)
                    .Select(y => y.GetType())
                    .SingleOrDefault();
                if (convertType is null) return null;
                return JsonSerializer.Deserialize(x.GetRawText(), convertType, options) as Event;
            })
            .Where(x => x is not null)
            .ToArray();
        reader = backup;
        return events;
    }


    public override void Write(Utf8JsonWriter writer, Event[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
            JsonSerializer.Serialize(writer, item, item.GetType(), options);
        writer.WriteEndArray();
    }
}
