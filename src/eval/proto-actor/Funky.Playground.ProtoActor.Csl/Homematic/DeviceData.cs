
using System.Text.Json.Serialization;

namespace Funky.Playground.ProtoActor.Homematic
{
    public record DeviceData(string Device, int Channel, string Type, Data Data);

    public record Data
    {
        public Data(long timestamp, object value, int status)
        {
            this.Timestamp = timestamp;
            this.Value = value;
            this.Status = status;
        }

        [JsonPropertyName("ts")]
        public long Timestamp { get; set; }

        [JsonPropertyName("v")]
        public object Value { get; set; }

        [JsonPropertyName("s")]
        public int Status { get; set; }
    }
}
