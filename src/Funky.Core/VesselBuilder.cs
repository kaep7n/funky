using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funky.Core
{
    public class VesselBuilder : IVesselBuilder
    {
        private DirectoryLoadContext loadContext;
        private Assembly assembly;

        public VesselBuilder()
            => this.Services.AddSingleton<IVessel, Vessel>();

        public IServiceCollection Services { get; } = new ServiceCollection();

        public IVessel Build() => this.Services.BuildServiceProvider()
                .GetService<IVessel>();

        public IVesselBuilder UseContentRoot(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var fullPath = Path.GetFullPath(path);

            if (!Directory.Exists(fullPath))
            {
                throw new ArgumentException("Directory does not exist.", nameof(path));
            }

            this.loadContext = new DirectoryLoadContext(fullPath);
            this.Services.AddSingleton<AssemblyLoadContext>(this.loadContext);

            return this;
        }

        public IVesselBuilder AddLogging()
        {
            this.Services.AddLogging();
            return this;
        }

        public IVesselBuilder AddLogging(Action<ILoggingBuilder> configure)
        {
            this.Services.AddLogging(configure);
            return this;
        }

        public IVesselBuilder UseAssembly(string assemblyName)
        {
            if (assemblyName is null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            this.assembly = this.loadContext.LoadFromAssemblyName(new AssemblyName(assemblyName));

            return this;
        }

        public IVesselBuilder UseFunk(string funkTypeName)
        {
            if (funkTypeName is null)
            {
                throw new ArgumentNullException(nameof(funkTypeName));
            }

            var type = this.assembly.GetType(funkTypeName);

            if(type == null)
            {
                throw new TypeNotFoundException($"Type {funkTypeName} not found in assembly {this.assembly.FullName}");
            }

            this.Services.AddSingleton(typeof(IFunk), type);

            return this;
        }

        public IVesselBuilder UseStartup(string startupTypeName)
        {
            if (startupTypeName is null)
            {
                throw new ArgumentNullException(nameof(startupTypeName));
            }

            var type = this.assembly.GetType(startupTypeName);

            if (type == null)
            {
                throw new TypeNotFoundException($"Type {startupTypeName} not found in assembly {this.assembly.FullName}");
            }

            if (!(Activator.CreateInstance(type) is IStartup instance))
            {
                throw new InvalidStartupException($"Type {type.FullName} does not implement interface");
            }

            instance.Configure(this.Services);

            return this;
        }
    }
}
