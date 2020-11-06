using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Funky.Core
{
    public class DirectoryLoadContext : AssemblyLoadContext
    {
        private readonly string directory;
        private readonly List<string> sharedAssemblies = new List<string>();

        public DirectoryLoadContext(string directory)
            : base($"Funky_{Guid.NewGuid()}", true)
        {
            this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.sharedAssemblies.Add("Funky.Core");
            this.sharedAssemblies.Add("Microsoft.Extensions.DependencyInjection");
            this.sharedAssemblies.Add("Microsoft.Extensions.DependencyInjection.Abstractions");
            this.sharedAssemblies.Add("Microsoft.Extensions.Logging.Abstractions");
            this.sharedAssemblies.Add("Microsoft.Extensions.Logging");
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName is null)
                throw new ArgumentNullException(nameof(assemblyName));

            if (this.sharedAssemblies.Contains(assemblyName.Name))
                return Default.LoadFromAssemblyName(assemblyName);

            var assemblyPath = Path.Combine(this.directory, $"{assemblyName.Name}.dll");

            return File.Exists(assemblyPath) ? this.LoadFromAssemblyPath(assemblyPath) : null;
        }
    }
}
