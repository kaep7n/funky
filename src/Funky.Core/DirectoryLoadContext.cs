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
            : base($"Funky_{Guid.NewGuid().ToString()}", true)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.directory = directory;
            this.sharedAssemblies.Add("Funky.Core");
            this.sharedAssemblies.Add("Microsoft.Extensions.DependencyInjection");
            this.sharedAssemblies.Add("Microsoft.Extensions.DependencyInjection.Abstractions");
            this.sharedAssemblies.Add("Microsoft.Extensions.Logging.Abstractions");
        }

        public IEnumerable<string> SharedAssemblies { get; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName is null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            if (this.sharedAssemblies.Contains(assemblyName.Name))
            {
                return Default.LoadFromAssemblyName(assemblyName);
            }

            var assemblyPath = Path.Combine(this.directory, $"{assemblyName.Name}.dll");

            if (File.Exists(assemblyPath))
            {
                return this.LoadFromAssemblyPath(assemblyPath);
            }
            else
            {
                return null;
            }
        }
    }
}
