using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Funky.Core
{
    public sealed class VesselBuilder : IVesselBuilder, IServiceCollection
    {
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly List<Assembly> assemblies = new List<Assembly>();
        private readonly List<AssemblyLoadContext> assemblyLoadContexts = new List<AssemblyLoadContext>();

        public VesselBuilder() => this.services.AddSingleton<IVessel>((p) => new Vessel(p.GetRequiredService<ILogger<Vessel>>()));

        public IVesselBuilder UseAssemblies(params string[] assemblyNames)
        {
            if (assemblyNames is null)
                throw new ArgumentNullException(nameof(assemblyNames));

            foreach (var assemblyName in assemblyNames)
            {
                var context = new DirectoryLoadContext(Directory.GetCurrentDirectory());

                var assembly = context.LoadFromAssemblyName(new AssemblyName(assemblyName));

                this.assemblies.Add(assembly);
                this.assemblyLoadContexts.Add(context);

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (typeof(IFunk).IsAssignableFrom(type))
                    {
                        // works for EmptyFunk
                    }
                    if (typeof(IFunk<>).IsAssignableFrom(type))
                    {
                        // should work for LoggingFunk but doesnt
                    }
                }
            }

            return this;
        }

        public ServiceDescriptor this[int index] { get => ((IList<ServiceDescriptor>)this.services)[index]; set => ((IList<ServiceDescriptor>)this.services)[index] = value; }

        public int Count => ((ICollection<ServiceDescriptor>)this.services).Count;

        public bool IsReadOnly => ((ICollection<ServiceDescriptor>)this.services).IsReadOnly;

        public void Add(ServiceDescriptor item) => ((ICollection<ServiceDescriptor>)this.services).Add(item);

        public IVessel Build() => this.services.BuildServiceProvider()
                .GetService<IVessel>();

        public void Clear() => ((ICollection<ServiceDescriptor>)this.services).Clear();

        public bool Contains(ServiceDescriptor item) => ((ICollection<ServiceDescriptor>)this.services).Contains(item);

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => ((ICollection<ServiceDescriptor>)this.services).CopyTo(array, arrayIndex);

        public IEnumerator<ServiceDescriptor> GetEnumerator() => ((IEnumerable<ServiceDescriptor>)this.services).GetEnumerator();

        public int IndexOf(ServiceDescriptor item) => ((IList<ServiceDescriptor>)this.services).IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => ((IList<ServiceDescriptor>)this.services).Insert(index, item);

        public bool Remove(ServiceDescriptor item) => ((ICollection<ServiceDescriptor>)this.services).Remove(item);

        public void RemoveAt(int index) => ((IList<ServiceDescriptor>)this.services).RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.services).GetEnumerator();
    }
}
