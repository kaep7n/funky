using Confluent.Kafka;
using System.Text.Json;

namespace Funky.Playground.Prototype
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
