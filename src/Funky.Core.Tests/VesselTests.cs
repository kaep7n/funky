using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
        public void Ctor_param_serviceProvider_should_throw_ArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Vessel(null, null));

            Assert.Equal($"Value cannot be null. (Parameter 'serviceProvider')", exception.Message);
        }

        [Fact]
        public void Ctor_param_funk_should_throw_ArgumentNullException()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var exception = Assert.Throws<ArgumentNullException>(() => new Vessel(serviceProvider, null));

            Assert.Equal($"Value cannot be null. (Parameter 'funk')", exception.Message);
        }

        [Fact]
        public void Ctor_should_create_instance()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var funk = new VesselTestFunk();
            var vessel = new Vessel(serviceProvider, funk);

            Assert.NotNull(vessel);
        }
    }
}
