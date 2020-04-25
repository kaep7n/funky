using Funky.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Bootstrapper
{
    public class BootstrapperService : IHostedService
    {
        private readonly IOptionsSnapshot<BootstrapperServiceOptions> options;
        private readonly IServiceProvider serviceProvider;

        public BootstrapperService(IOptionsSnapshot<BootstrapperServiceOptions> options, IServiceProvider serviceProvider)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var funks = this.serviceProvider.GetServices<IFunk>();

            foreach (var funk in funks)
            {
                await funk.EnableAsync().ConfigureAwait(false);
                await funk.DisableAsync()
                    .ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
