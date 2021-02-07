using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Prototype
{
    public class Manager : IHostedService
    {
        private static readonly string[] brokers = new[] { "localhost:9092", "localhost:9093", "localhost:9094" };
        private readonly KafkaConsumer<Temperature> tempConsumer;
        private readonly KafkaProducer<Temperature> tempProducer;
        private readonly ILogger<Manager> logger;
        private Task tempConsumerTask;
        private System.Timers.Timer produceTimer;

        public Manager(ILogger<Manager> logger, ILogger<KafkaProducer<Temperature>> tempProducerLogger)
        {
            this.produceTimer = new System.Timers.Timer(1000);
            this.produceTimer.Elapsed += this.ProduceTimer_Elapsed;
            this.tempConsumer = new KafkaConsumer<Temperature>(brokers, "temp", "1");
            this.tempProducer = new KafkaProducer<Temperature>(brokers, "temp", tempProducerLogger);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async void ProduceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var r = new Random();

            await this.SendAsync(new Temperature(r.NextDouble()))
                .ConfigureAwait(false);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.tempConsumer.EnableAsync(cancellationToken)
                .ConfigureAwait(false);

            this.tempConsumerTask = Task.Run(async () =>
            {
                await foreach (var evt in this.tempConsumer.ReadAllAsync())
                {
                    this.logger.LogInformation($"received {evt.Value}");
                }
            });

            this.produceTimer.Start();
        }

        public async Task SendAsync(Temperature evt)
        {
            this.logger.LogInformation($"sending {evt.Value}");
            await this.tempProducer.ProduceAsync(evt)
                .ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.produceTimer.Stop();

            await this.tempConsumer.DisableAsync()
                .ConfigureAwait(false);

            this.tempProducer.Dispose();
        }
    }
}
