﻿using Funky.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

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

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            var assembly = this.context.LoadFromAssemblyName(funkDef.TypeName.Assembly);
            var funkType = assembly.GetType(funkDef.TypeName.FullName);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(typeof(IFunk), funkType);

            this.serviceProvider = serviceCollection.BuildServiceProvider();

            return Task.CompletedTask;
        }

        public async Task ConsumeAsync()
        {
            var funk = this.serviceProvider.GetRequiredService<IFunk>();

            await funk.ExecuteAsync()
                .ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
