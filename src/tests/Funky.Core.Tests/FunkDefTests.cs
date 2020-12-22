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
            var expectedTypeName = new TypeName(type.AssemblyQualifiedName);
            var actualDefinition = new FunkDef(type.AssemblyQualifiedName);

            Assert.Equal(expectedTypeName, actualDefinition.TypeName);
        }
    }
}