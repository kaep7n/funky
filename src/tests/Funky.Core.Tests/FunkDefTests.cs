using Funky.Fakes;
using System.Linq;
using Xunit;

namespace Funky.Core.Tests
{
    public class FunkDefTests
    {
        [Fact]
        public void Ctor_should_parse_type_name()
        {
            var type = typeof(EmptyFunk);
            var expectedTypeName = new TypeName(type.AssemblyQualifiedName);
            var actualDefinition = new FunkDef(type.AssemblyQualifiedName, "topic");

            Assert.Equal(expectedTypeName, actualDefinition.TypeName);
        }
    }
}