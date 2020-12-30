using System;
using System.Linq;

namespace Funky.Fakes
{
    public class FakeServiceProvider : IServiceProvider
    {
        private readonly object[] services;

        public FakeServiceProvider(params object[] services)
            => this.services = services ?? throw new ArgumentNullException(nameof(services));

        public object GetService(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                return this.services.FirstOrDefault(s => s.GetType().GetInterfaces().Any(i => i == serviceType));
            }

            // TODO: if interface is requested this won't work
            // TODO: get a collection of services won't work
            return this.services.FirstOrDefault(s => s.GetType() == serviceType);
        }
    }
}
