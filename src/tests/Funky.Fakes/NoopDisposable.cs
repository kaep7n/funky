using System;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    /// <summary>
    /// A singleton disposable that does nothing when disposed.
    /// </summary>
    public sealed class NoopDisposable : IDisposable, IAsyncDisposable
    {
        private NoopDisposable()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public ValueTask DisposeAsync() => new ValueTask();

        /// <summary>
        /// Gets the instance of <see cref="NoopDisposable"/>.
        /// </summary>
        public static NoopDisposable Instance { get; } = new NoopDisposable();
    }
}
