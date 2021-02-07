using Confluent.Kafka;
using System;
using System.Text.Json;

namespace Funky.Playground.Prototype
{
    public class JsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<T>(data);
    }
}
