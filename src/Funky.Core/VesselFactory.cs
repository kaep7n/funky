using System;

namespace Funky.Core
{
    public class VesselFactory
    {
        private readonly IServiceProvider serviceProvider;

        public VesselFactory(IServiceProvider serviceProvider)
            => this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IVessel Create(FunkDef funkDef)
        {
            if (funkDef is null)
                throw new ArgumentNullException(nameof(funkDef));

            return new Vessel(funkDef);
        }
    }
}
