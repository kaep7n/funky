using Funky.Playground.ProtoActor.Homematic;
using Microsoft.Extensions.Logging;
using Proto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.ProtoActor
{
    public class HomaticRoot : IActor
    {
        private readonly ILogger<HomaticRoot> logger;
        private PID deviceController;

        public HomaticRoot(ILogger<HomaticRoot> logger)
        {
            logger.LogInformation("homematic root created");
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task ReceiveAsync(IContext context)
        {
            if(context.Message is Started)
            {
                Console.WriteLine("homematic root started, creating device controller", Color.LightGreen);
                this.deviceController = context.Spawn(Props.FromProducer(() => new DeviceController()));
            }
            if(context.Message is DeviceData msg)
            {
                Console.WriteLine($"forwarding device data to {msg.Device}", Color.LightGreen);
                context.Forward(this.deviceController);
            }

            return Task.CompletedTask;
        }
    }
}
