using System;
using System.Threading.Tasks;
using Funky.Messaging;
using Xunit;

namespace Funky.Core.Tests
{
    [AcceptsTopic("/test")]
    public class VesselTestFunk : IFunk
    {
        public bool WasExecuted { get; private set; }

        public ValueTask ExecuteAsync(object _)
        {
            this.WasExecuted = true;

            return new ValueTask();
        }
    }

    public class VesselTests
    {
        [Fact]
        public void Ctor_param_funk_should_throw_ArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Vessel(null));

            Assert.Equal($"Value cannot be null. (Parameter 'funk')", exception.Message);
        }

        [Fact]
        public void Ctor_should_create_instance()
        {
            var funk = new VesselTestFunk();
            var vessel = new Vessel(funk);

            Assert.NotNull(vessel);
        }

        [Fact]
        public async Task ConsumeAsync_should_execute_funk()
        {
            var funk = new VesselTestFunk();
            var vessel = new Vessel(funk);

            await vessel.StartAsync();

            var topic = TopicBuilder.Root("test").Build();
            var payload = new EmptyPayload();

            var message = new Message(topic, payload);

            await vessel.ConsumeAsync(message)
                .ConfigureAwait(false);

            Assert.True(funk.WasExecuted);
        }
    }
}
