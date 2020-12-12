using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Funky.Fakes
{
    public class FakeOptionsMonitor<T> : IOptionsMonitor<T>
    {
        private readonly T value;
        private readonly Dictionary<string, T> namedValues = new();
        private Action<T, string> changeListener;

        public FakeOptionsMonitor(T value)
        {
            this.value = value;
        }

        public T CurrentValue => this.value;

        public T Get(string name)
        {
            return this.namedValues[name];
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            if (listener is null)
                throw new ArgumentNullException(nameof(listener));

            this.changeListener = listener;

            return NoopDisposable.Instance;
        }
    }
}