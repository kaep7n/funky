using Funky.Playground.ProtoActor.Homematic;
using Proto;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.ProtoActor
{
    public class Device : IActor
    {
        private readonly string identifier;
        private DeviceInformation information;

        public Device(string identifier)
        {
            if (identifier is null)
                throw new ArgumentNullException(nameof(identifier));

            Console.WriteLine($"device {identifier} created", Color.LightGoldenrodYellow);
            this.identifier = identifier;
        }

        public async Task ReceiveAsync(IContext context)
        {
            if(context.Message is Started)
            {
                Console.WriteLine($"device {identifier} started", Color.LightGoldenrodYellow);

                var httpClient = new HttpClient();

                this.information = await httpClient.GetFromJsonAsync<DeviceInformation>($"http://192.168.2.101:2121/device/{this.identifier}");

                Console.WriteLine($"received additional device information {Environment.NewLine}{this.information}", Color.LightGoldenrodYellow);
            }
            if(context.Message is DeviceData msg)
            {
                Console.WriteLine($"{this.identifier} | received message: {msg}", Color.LightGoldenrodYellow);
                context.System.EventStream.Publish(context.Message);
            }
        }
    }
}
