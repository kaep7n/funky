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
    //public sealed class ProducerFunk : IFunk
    //{
        //private readonly IProducer<ConfigurationChanged> producer;
        //private readonly ILogger<ProducerFunk> logger;

        //public ProducerFunk(IProducer<ConfigurationChanged> producer, ILogger<ProducerFunk> logger)
        //{
        //    this.producer = producer ?? throw new ArgumentNullException(nameof(producer));
        //    this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        //public async ValueTask ExecuteAsync()
        //{
        //    var configuration = new Dictionary<string, string>()
        //    {
        //        { "weather:username", "kaeptn" },
        //        { "weather:host", "http://weatherservice.de/api" }
        //    };

        //    this.logger.LogInformation("producing configuration changed");

        //    await this.producer.ProduceAsync(new ConfigurationChanged(DateTimeOffset.UtcNow, configuration))
        //        .ConfigureAwait(false);

        //    this.logger.LogInformation("produced configuration changed");
        //}
    //}
}
