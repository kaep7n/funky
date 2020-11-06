using Funky.Core;
using Funky.Core.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Funky.Bootstrapper.TestHost3
{
    public class ConsumerFunk : IFunk<ConfigurationChanged>
    {
        private readonly ILogger<ConsumerFunk> logger;

        public ConsumerFunk(ILogger<ConsumerFunk> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ValueTask ExecuteAsync(ConfigurationChanged message)
        {
            this.logger.LogInformation($"received event {message}");

            return ValueTask.CompletedTask;
        }
    }
}
