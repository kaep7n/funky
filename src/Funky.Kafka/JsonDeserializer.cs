using Confluent.Kafka;
using System;
using System.Text.Json;

namespace Funky.Events.Kafka
{
    public class JsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<T>(data);
    }
}
