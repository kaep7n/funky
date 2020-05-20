using System;
using System.Threading.Tasks;
using Xunit;

namespace Funky.Core.Tests
{
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
    }
}
