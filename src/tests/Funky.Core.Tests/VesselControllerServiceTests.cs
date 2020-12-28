using Funky.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Funky.Core.Tests
{
    public class VesselControllerServiceTests
    {
        private readonly ILogger<VesselControllerService> logger = NullLogger<VesselControllerService>.Instance;

        [Fact]
        public void Ctor_should_create_instance()
        {
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions());
        
            var service = new VesselControllerService(optionsMonitor, this.logger);

            Assert.NotNull(service);
        }

        [Fact]
        public async Task StartAsync_with_empty_options_should_start()
        {
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions());
        
            var service = new VesselControllerService(optionsMonitor, this.logger);

            await service.StartAsync(CancellationToken.None)
                .ConfigureAwait(false);

            Assert.Empty(service.Vessels);
        }

        [Fact]
        public async Task StartAsync_with_one_funk_without_dependencies_should_start()
        {
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions
            {
                FunkDefs = new [] { "Funky.Fakes.EmptyFunk, Funky.Fakes" }
            });
        
            var service = new VesselControllerService(optionsMonitor, this.logger);

            await service.StartAsync(CancellationToken.None)
                .ConfigureAwait(false);

            Assert.Single(service.Vessels);
        }

        [Fact]
        public async Task StartAsync_with_one_funk_without_dependencies_should_accept_message()
        {
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions
            {
                FunkDefs = new[] { "Funky.Fakes.EmptyFunk, Funky.Fakes" }
            });

            var service = new VesselControllerService(optionsMonitor, this.logger);

            await service.StartAsync(CancellationToken.None)
                .ConfigureAwait(false);

            var vessel = Assert.Single(service.Vessels);

            await vessel.ConsumeAsync();
        }
    }
}