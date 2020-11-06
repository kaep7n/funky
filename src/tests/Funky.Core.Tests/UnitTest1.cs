using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Funky.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var builder = new VesselBuilder();

            builder.UseAssemblies("Funky.Fakes");
            builder.AddLogging();

            var vesssel = builder.Build();

            Assert.NotNull(vesssel);
        }
    }
}
