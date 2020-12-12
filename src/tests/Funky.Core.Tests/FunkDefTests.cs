using Funky.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Funky.Core.Tests
{
    public class FunkDefTests
    {
        [Fact]
        public void Ctor_()
        {
            var type = typeof(LoggingFunk);
            var definition = new FunkDef(type.AssemblyQualifiedName);

            Assert.Equal(type.FullName, definition.Type);
            Assert.Equal(type.Assembly.GetName().ToString(), definition.Assembly.ToString());
        }
    }
}