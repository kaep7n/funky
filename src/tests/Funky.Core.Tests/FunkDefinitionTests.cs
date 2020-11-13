using Funky.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Funky.Core.Tests
{
    public class FunkDefinitionTests
    {
        [Fact]
        public void Ctor_()
        {
            var type = typeof(LoggingFunk);
            var definition = new FunkDefinition(type.AssemblyQualifiedName);

            Assert.Equal(type.FullName, definition.Type);
            Assert.Equal(type.Assembly.GetName().ToString(), definition.Assembly.ToString());
        }

        [Fact]
        public void Ctor_implicit_conversion()
        {
            var type = typeof(LoggingFunk);
            var definition = (FunkDefinition)type.AssemblyQualifiedName;

            Assert.Equal(type.FullName, definition.Type);
            Assert.Equal(type.Assembly.GetName().ToString(), definition.Assembly.ToString());
        }

        [Fact]
        public void Configuration_JsonFile()
        {
            var type = typeof(LoggingFunk);

            File.WriteAllText("test.json", @$"{{ ""definition"": ""{type.AssemblyQualifiedName}"" }}");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("test.json")
                .Build();

            FunkDefinition definition = configuration.GetValue<string>("definition");

            Assert.Equal(type.FullName, definition.Type);
            Assert.Equal(type.Assembly.GetName().ToString(), definition.Assembly.ToString());
        }

        [Fact]
        public void Configuration_Environment()
        {
            var type = typeof(LoggingFunk);

            Environment.SetEnvironmentVariable("definition", type.AssemblyQualifiedName);

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            FunkDefinition definition= configuration.GetValue<string>("definition");

            Assert.Equal(type.FullName, definition.Type);
            Assert.Equal(type.Assembly.GetName().ToString(), definition.Assembly.ToString());
        }

        public class Configuration
        {
            public string Definition { get; set; }
        }
    }
}