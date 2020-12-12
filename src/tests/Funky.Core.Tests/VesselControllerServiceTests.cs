using Funky.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
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
    }
}