using System.Text.Json;
using System.Text.Json.Serialization;
using LineDevSdk.DTO.Commons.Messages;
using LineDevSdk.Utilities;

namespace LineDevSdk.Converter;

/// <summary>
/// インターフェイス型プロパティを実際の型に変換するJsonConverter
/// </summary>
/// <typeparam name="T"></typeparam>
public class MessageConverter : JsonConverter<IMessage>
{
    public override IMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var backup = reader;
        using var doc = JsonDocument.ParseValue(ref reader);
        var typeValue = doc.RootElement.EnumerateObject()
            .Where(x => x.Name.Equals("type", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Value.GetString())
            .SingleOrDefault();
        var convertType = Polymorphism.CreatePolymorphismArray<IMessage>()
            .Where(x => x.Type == typeValue)
            .Select(x => x.GetType())
            .SingleOrDefault();
        if(convertType is null) return null;
        reader = backup;
        return JsonSerializer.Deserialize(ref reader, convertType, options) as IMessage;
    }


    public override void Write(Utf8JsonWriter writer, IMessage value, JsonSerializerOptions options)
    {
        var realType = value.GetType();
        JsonSerializer.Serialize(writer, value, realType, options);
    }
}
