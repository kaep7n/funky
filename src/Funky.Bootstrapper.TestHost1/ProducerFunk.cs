using Funky.Core;
using Funky.Core.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Funky.Bootstrapper.TestHost1
{
    public sealed class ProducerFunk : IFunk, IDisposable
    {
        private readonly Timer timer;
        private readonly IProducer<ConfigurationChanged> producer;
        private readonly ILogger<ProducerFunk> logger;

        public ProducerFunk(IProducer<ConfigurationChanged> producer, ILogger<ProducerFunk> logger)
        {
            this.timer = new Timer(100);
            this.timer.Elapsed += this.Timer_Elapsed;
            this.producer = producer ?? throw new ArgumentNullException(nameof(producer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public Task DisableAsync()
        {
            this.timer.Stop();
            
            return Task.CompletedTask;
        }

        public Task EnableAsync()
        {
            this.timer.Start();

            return Task.CompletedTask;
        }

        public void Dispose() => this.timer.Dispose();
        
        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var configuration = new Dictionary<string, string>()
            {
                { "weather:username", "kaeptn" },
                { "weather:host", "http://weatherservice.de/api" }
            };

            this.logger.LogInformation("producing configuration changed");

            await this.producer.ProduceAsync(new ConfigurationChanged(DateTimeOffset.UtcNow, configuration))
                .ConfigureAwait(false);

            this.logger.LogInformation("produced configuration changed");
        }
    }
}
