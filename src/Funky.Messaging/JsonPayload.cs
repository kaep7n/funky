using System;

namespace Funky.Messaging
{
    public class JsonPayload : IPayload
    {
        private readonly object value;

        private JsonPayload() { }

        public JsonPayload(object value) => this.value = value ?? throw new ArgumentNullException(nameof(value));

        public object Get() => this.value;
    }
}
