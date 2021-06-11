using Funky.Playground.ProtoActor.Homematic;
using Proto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.ProtoActor
{
    public class DeviceController : IActor
    {
        private readonly Dictionary<string, PID> devices = new();

        public DeviceController()
        {
            Console.WriteLine("device controller created", Color.LightGreen);
        }

        public async Task ReceiveAsync(IContext context)
        {
            if (context.Message is Started)
            {
                Console.WriteLine("device controller started, getting devices", Color.LightSkyBlue);

                var httpClient = new HttpClient();

                var response = await httpClient.GetFromJsonAsync<DeviceQueryResult>("http://192.168.2.101:2121/device");

                foreach (var link in response.Links)
                {
                    if (link.IsParentRef)
                        continue;

                    Console.WriteLine($"creating device {link.Href}", Color.LightSkyBlue);

                    var pid = context.Spawn(Props.FromProducer(() => new Device(link.Href)));
                    this.devices.Add(link.Href, pid);
                }
            }
            if(context.Message is DeviceData data)
            {
                context.Forward(this.devices[data.Device]);
            }
        }
    }
}
