using Funky.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Funky.Core
{
    public sealed class Vessel : IVessel
    {
        private readonly AssemblyLoadContext context = new DirectoryLoadContext(Directory.GetCurrentDirectory());
        private readonly List<IConsumer> consumers = new();
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

            var sysEventsConsumer = this.consumerFactory.Create<SysEvent>("funky.sys");

            Task.Run(async () =>
            {
                await foreach (var evt in sysEventsConsumer.ReadAllAsync()
                    .ConfigureAwait(false))
                {
                    var funk = this.serviceProvider.GetService<IFunk>();

                    await funk.ExecuteAsync()
                        .ConfigureAwait(false);
                }
            });

            this.consumers.Add(sysEventsConsumer);
        }
    }
}
