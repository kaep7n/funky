using Funky.Playground.ProtoActor.Homematic;
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
    public class HomematicRoot : IActor
    {
        private PID deviceController;

        public HomematicRoot()
        {
            Console.WriteLine("homematic root created", Color.LightGreen);
        }

        public Task ReceiveAsync(IContext context)
        {
            if(context.Message is Started)
            {
                Console.WriteLine("homematic root started, creating device controller", Color.LightSkyBlue);
                this.deviceController = context.Spawn(Props.FromProducer(() => new DeviceController()));
            }
            if(context.Message is DeviceData)
            {
                context.Forward(this.deviceController);
            }

            return Task.CompletedTask;
        }
    }
}
