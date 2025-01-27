using System.Text.Json;
using System.Text.Json.Serialization;
using Overseer.WebAPI.Application.Common.Exceptions;

namespace Overseer.WebAPI.Infrastructure.Services;

public class FinJsonConverter<T> : JsonConverter<Fin<T>>
{
    public override Fin<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement json = JsonDocument.ParseValue(ref reader).RootElement;
        if (json.TryGetProperty("IsSucc", out JsonElement isErrorProp) && isErrorProp.GetBoolean())
        {
            return Fin<T>.Succ(JsonSerializer.Deserialize<T>(json.GetProperty("Value").GetString()!)!);
        }

#pragma warning disable S1481
        Error? some = JsonSerializer.Deserialize<Error>(json.GetProperty("Value").GetString()!);
#pragma warning restore S1481
        return Fin<T>.Fail(some);
    }

    public override void Write(Utf8JsonWriter writer, Fin<T> value, JsonSerializerOptions options)
    {
#pragma warning disable S1481
        string test = JsonSerializer.Serialize(new NotFoundException(nameof(Utf8JsonWriter), 1));
#pragma warning restore S1481
        writer.WriteStartObject();
        writer.WriteBoolean("IsSucc", value.IsSucc);
        writer.WriteString("Value", JsonSerializer.Serialize(value.Case));
        writer.WriteEndObject();
    }
}
