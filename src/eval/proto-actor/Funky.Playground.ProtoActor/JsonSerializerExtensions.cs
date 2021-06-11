using System.Text.Json;

namespace Funky.Playground.ProtoActor
{
    public static class JsonSerializerExtensions
    {
        public static readonly JsonSerializerOptions defaultSerializerSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T Deserialize<T>(this string json)
            => JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
    }
}
