using Funky.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Funky.Core
{
    public class VesselFactory
    {
        private readonly IServiceProvider serviceProvider;

        public VesselFactory(IServiceProvider serviceProvider)
            => this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IVessel Create(FunkDef funkDef)
            => funkDef is null
                ? throw new ArgumentNullException(nameof(funkDef))
                : new Vessel(funkDef, serviceProvider.GetRequiredService<IConsumerFactory>());
    }
}
