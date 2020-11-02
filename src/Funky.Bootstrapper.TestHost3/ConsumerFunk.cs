using Funky.Core;
using Funky.Core.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Funky.Bootstrapper.TestHost3
{
    public class ConsumerFunk : IFunk
    {
        private readonly IConsumer<ConfigurationChanged> consumer;
        private readonly ILogger<ConsumerFunk> logger;

        public ConsumerFunk(IConsumer<ConfigurationChanged> consumer, ILogger<ConsumerFunk> logger)
        {
            this.consumer = consumer ?? throw new System.ArgumentNullException(nameof(consumer));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public Task DisableAsync() => this.consumer.DisableAsync();

        public async Task EnableAsync()
        {
            await this.consumer.EnableAsync();

            _ = Task.Run(async () =>
              {
                  await foreach (var e in this.consumer.ReadAllAsync())
                  {
                      this.logger.LogInformation($"received event {e}");
                  }
              });
        }
    }
}
