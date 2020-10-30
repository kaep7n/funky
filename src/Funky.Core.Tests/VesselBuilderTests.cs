using System;
using System.IO;
using Xunit;

namespace Funky.Core.Tests
{
    public class VesselBuilderTests
    {
        [Fact]
        public void Should_throw_ArgumentNullException_on_UseContentRoot()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new VesselBuilder().UseContentRoot(null));

            Assert.Equal($"Value cannot be null. (Parameter 'path')", exception.Message);
        }

        [Fact]
        public void Should_throw_ArgumentException_on_non_existing_directory_on_UseContentRoot()
        {
            var nonExistingDirectory = Path.GetFullPath("NonExistingDirectory");

            var exception = Assert.Throws<ArgumentException>(() => new VesselBuilder().UseContentRoot(nonExistingDirectory));

            Assert.Equal($"Path '{nonExistingDirectory}' does not exist (Parameter 'path')", exception.Message);
        }

        [Fact]
        public void Should_throw_ArgumentNullException_on_UseAssembly()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new VesselBuilder().UseAssembly(null));

            Assert.Equal($"Value cannot be null. (Parameter 'assemblyName')", exception.Message);
        }

        [Fact]
        public void Should_throw_ArgumentNullException_UseFunk()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new VesselBuilder().UseFunk(null));

            Assert.Equal($"Value cannot be null. (Parameter 'funkTypeName')", exception.Message);
        }

        [Fact]
        public void Should_throw_ArgumentNullException_on_UseStartup()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new VesselBuilder().UseStartup(null));

            Assert.Equal($"Value cannot be null. (Parameter 'startupTypeName')", exception.Message);
        }

        [Fact]
        public void Should_throw_FileNotFoundExceptionon_on_not_existing_assembly()
            => Assert.Throws<FileNotFoundException>(() => new VesselBuilder()
                    .UseContentRoot("../../../Resources")
                    .UseAssembly("Assembly.Does.Not.Exist"));

        [Fact]
        public void Should_throw_TypeNotFoundException_on_not_existing_type()
            => Assert.Throws<TypeNotFoundException>(() => new VesselBuilder()
                    .UseContentRoot("../../../Resources")
                    .UseAssembly("Funky.Fakes")
                    .UseFunk("Type.Does.Not.Exist"));

        [Fact]
        public void Should_throw_ArgumentException_on_type_that_does_not_implemented_IFunk()
        {
            var exception = Assert.Throws<ArgumentException>(() => new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.NoFunk")
                .Build());

            Assert.Equal("Implementation type 'Funky.Fakes.NoFunk' can't be converted to service type 'Funky.Core.IFunk'", exception.Message);
        }

        [Fact]
        public void Should_throw_InvalidStartupException_()
            => Assert.Throws<InvalidStartupException>(() => new VesselBuilder()
                    .UseContentRoot("../../../Resources")
                    .UseAssembly("Funky.Fakes")
                    .UseFunk("Funky.Fakes.EmptyFunk")
                    .UseStartup("Funky.Fakes.NoStartup")
                    .AddLogging()
                    .Build());

        [Fact]
        public void Should_create_vessel_on_build()
        {
            var vessel = new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.EmptyFunk")
                .AddLogging()
                .Build();

            Assert.NotNull(vessel);
        }

        [Fact]
        public void Should_create_vessel_with_empty_startup_on_build()
        {
            var vessel = new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.EmptyFunk")
                .UseStartup("Funky.Fakes.EmptyStartup")
                .AddLogging()
                .Build();

            Assert.NotNull(vessel);
        }

        [Fact]
        public void Should_create_vessel_with_logging_funk_and_empty_startup_on_build()
        {
            var vessel = new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.LoggingFunk")
                .UseStartup("Funky.Fakes.EmptyStartup")
                .AddLogging()
                .Build();

            Assert.NotNull(vessel);
        }

        [Fact]
        public void Should_create_vessel_with_logging_funk_and_logging_startup_on_build()
        {
            var vessel = new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.LoggingFunk")
                .UseStartup("Funky.Fakes.LoggingStartup")
                .AddLogging()
                .Build();

            Assert.NotNull(vessel);
        }

        [Fact]
        public void Should_create_vessel_with_logging_startup_on_build()
        {
            var vessel = new VesselBuilder()
                .UseContentRoot("../../../Resources")
                .UseAssembly("Funky.Fakes")
                .UseFunk("Funky.Fakes.LoggingFunk")
                .UseStartup("Funky.Fakes.LoggingStartup")
                .Build();

            Assert.NotNull(vessel);
        }
    }
}
