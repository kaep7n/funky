using Funky.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Funky.Core
{
    public sealed class Vessel : IVessel
    {
        private readonly AssemblyLoadContext context = new DirectoryLoadContext(Directory.GetCurrentDirectory());
        private readonly FunkDef funkDef;
        private readonly IConsumerFactory consumerFactory;
        private IServiceProvider serviceProvider;

        public Vessel(FunkDef funkDef, IConsumerFactory consumerFactory)
        {
            this.funkDef = funkDef ?? throw new ArgumentNullException(nameof(funkDef));
            this.consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
        }

        public void Initialize()
        {
            var assembly = this.context.LoadFromAssemblyName(funkDef.TypeName.Assembly);
            var funkType = assembly.GetType(funkDef.TypeName.FullName);

            var services = new ServiceCollection();
            services.AddTransient(typeof(IFunk), funkType);

            var startupAttribute = funkType.GetCustomAttribute(typeof(StartupAttribute), true) as StartupAttribute;

            if (startupAttribute is not null)
            {
                var startup = Activator.CreateInstance(startupAttribute.StartupType) as IStartup;
                startup?.Configure(services);
            }

            this.serviceProvider = services.BuildServiceProvider();
        }
    }
}
