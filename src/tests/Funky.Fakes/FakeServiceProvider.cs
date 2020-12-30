using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Fakes
{
    public class FakeServiceProvider : IServiceProvider
    {
        private readonly object[] services;

        public FakeServiceProvider(params object[] services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public object GetService(Type serviceType)
        {
            // TODO: if interface is requested this won't work
            // TODO: get a collection of services won't work
            return this.services.FirstOrDefault(s => s.GetType() == serviceType);
        }
    }
}
