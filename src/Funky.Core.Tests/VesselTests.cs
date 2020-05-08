using System;
using System.Threading.Tasks;
using Funky.Fakes;
using Funky.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Funky.Core.Tests
{
    public class VesselTestFunk : IFunk
    {
        public Task DisableAsync() => Task.CompletedTask;
        public Task EnableAsync() => Task.CompletedTask;
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
        public async Task ConsumeAsync_()
        {
            var funk = new LoggingFunk(NullLogger<LoggingFunk>.Instance);
            var vessel = new Vessel(funk);

            await vessel.StartAsync();

            var topic = TopicBuilder.Root("test").Build();
            var payload = new JsonPayload(new LogEntry
            {
                Level = LogLevel.Information,
                Message = "just a test"
            });
            var message = new Message(topic, payload);

            await vessel.ConsumeAsync(message)
                .ConfigureAwait(false);
        }
    }
}
