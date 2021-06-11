using Funky.Playground.ProtoActor.Homematic;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Proto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var system = new ActorSystem();

            var props = Props.FromProducer(() => new HomematicRoot());
            var pid = system.Root.Spawn(props);

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("funky")
                    .WithTcpServer("192.168.2.101")
                    .Build())
                .Build();

            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("#")
                .Build());

            mqttClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("disconnected", Color.MediumVioletRed);
            });

            mqttClient.UseConnectedHandler(e =>
            {
                Console.WriteLine("connected", Color.LightGreen);
            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var topicPaths = e.ApplicationMessage.Topic.Split("/");

                if(topicPaths.Length > 2)
                {
                    var device = topicPaths[2];

                    system.Root.Send(pid, new DeviceData(device, e.ApplicationMessage.Payload));
                }
            });

            await mqttClient.StartAsync(options);

            Console.ReadLine();
        }
    }
}
