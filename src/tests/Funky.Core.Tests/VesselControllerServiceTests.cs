using Funky.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Funky.Core.Tests
{
    public class VesselControllerServiceTests
    {
        [Fact]
        public void Ctor_()
        {
            var logger = NullLogger<VesselControllerService>.Instance;
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions());
        
            var service = new VesselControllerService(optionsMonitor, logger);
        }

        [Fact]
        public async Task StartAsync_with_empty_options_should_start()
        {
            var logger = NullLogger<VesselControllerService>.Instance;
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions());
        
            var service = new VesselControllerService(optionsMonitor, logger);

            await service.StartAsync(CancellationToken.None)
            .ConfigureAwait(false);
        }

        [Fact]
        public async Task StartAsync_with_one_funk_without_dependencies_should_start()
        {
            var logger = NullLogger<VesselControllerService>.Instance;
            var optionsMonitor = new FakeOptionsMonitor<VesselControllerServiceOptions>(new VesselControllerServiceOptions
            {
                FunkDefs = new [] { "Funky.Fakes.EmptyFunk, Funky.Fakes" }
            });
        
            var service = new VesselControllerService(optionsMonitor, logger);

            await service.StartAsync(CancellationToken.None)
            .ConfigureAwait(false);
        }
    }
}