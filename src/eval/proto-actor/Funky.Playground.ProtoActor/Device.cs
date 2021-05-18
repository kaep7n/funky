using Proto;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor
{
    public class Device : IActor
    {
        private readonly DeviceInformation deviceInformation;

        public Device(DeviceInformation deviceInformation)
        {
            if (deviceInformation is null)
                throw new ArgumentNullException(nameof(deviceInformation));
            this.deviceInformation = deviceInformation;
            Console.WriteLine($"Device {this.deviceInformation.Identifier} created");
        }

        public Task ReceiveAsync(IContext context)
        {
            Console.WriteLine($"Device received message {context.Message}");

            return Task.CompletedTask;
        }
    }
}
