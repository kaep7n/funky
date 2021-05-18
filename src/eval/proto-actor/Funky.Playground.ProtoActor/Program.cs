using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dictionary = new Dictionary<string, PID>();

            var system = new ActorSystem();

            var httpClient = new HttpClient();

            var response = await httpClient.GetFromJsonAsync<JsonElement>("http://192.168.2.101:2121/device");
            var links = response
                .GetProperty("~links")
                .GetRawText()
                .Deserialize<IEnumerable<Link>>();

            foreach (var link in links)
            {
                if (link.Href == "..")
                    continue;

                var device = await httpClient.GetFromJsonAsync<DeviceInformation>($"http://192.168.2.101:2121/{link.Rel}/{link.Href}");

                var props = Props.FromProducer(() => new Device(device));
                var pid = system.Root.Spawn(props);
                dictionary.Add(link.Href, pid);
            }

            Console.ReadLine();
        }
    }
}
