using System.Text.Json;
using System.Text.Json.Serialization;
using Overseer.FluentExtensions.Error;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Common.Exceptions;

namespace Overseer.WebAPI.Infrastructure.Services;

public class FinJsonConverter<T> : JsonConverter<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement json = JsonDocument.ParseValue(ref reader).RootElement;
        if (json.TryGetProperty("IsSucc", out JsonElement isErrorProp) && isErrorProp.GetBoolean())
        {
            return Result<T>.Success(JsonSerializer.Deserialize<T>(json.GetProperty("Value").GetString()!)!);
        }

#pragma warning disable S1481
        Error some = JsonSerializer.Deserialize<Error>(json.GetProperty("Value").GetString()!);
#pragma warning restore S1481
        return Result<T>.Failure(some);
    }

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
#pragma warning disable S1481
        string test = JsonSerializer.Serialize(new NotFoundException(nameof(Utf8JsonWriter), 1));
#pragma warning restore S1481
        writer.WriteStartObject();
        writer.WriteBoolean("IsSucc", value.IsSuccessful);
        writer.WriteString("Value", JsonSerializer.Serialize(value.Value));
        writer.WriteEndObject();
    }
}
