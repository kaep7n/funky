using System;

namespace Funky.Messaging
{
    public class JsonPayload : IPayload
    {
        private readonly object value;

        public JsonPayload(object value) => this.value = value ?? throw new System.ArgumentNullException(nameof(value));

        public object GetData() => this.value;

        public Type GetDataType() => this.value.GetType();
    }
}
