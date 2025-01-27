using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Hybrid;

namespace Overseer.WebAPI.Infrastructure.Services;

public class FinHybridCacheSerializer<T> : IHybridCacheSerializer<T>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public FinHybridCacheSerializer()
    {
        Type finType = typeof(T);
        if (finType.IsGenericType && finType.GetGenericTypeDefinition() == typeof(Fin<>))
        {
            Type innerType = finType.GetGenericArguments()[0];

            // Создаём конвертер с помощью рефлексии
            Type converterType = typeof(FinJsonConverter<>).MakeGenericType(innerType);
            object? converterInstance = Activator.CreateInstance(converterType);

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { (JsonConverter)converterInstance! }
            };
        }
        else
        {
            throw new InvalidOperationException($"{typeof(T)} не является типом Fin<A>.");
        }
    }


    // Десериализация данных
    public T Deserialize(ReadOnlySequence<byte> source)
    {
        // Конвертируем байтовую последовательность в строку и десериализуем её в объект
        byte[] byteArray = source.ToArray();
        return JsonSerializer.Deserialize<T>(byteArray, _jsonSerializerOptions)!;
    }

    // Сериализация данных
    public void Serialize(T value, IBufferWriter<byte> target)
    {
        // Сериализуем объект в байтовый массив
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonSerializerOptions);

        // Записываем байтовый массив в буфер
        target.Write(jsonBytes);
    }
}
