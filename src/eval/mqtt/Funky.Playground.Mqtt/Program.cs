using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Playground.Mqtt
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup and start a managed MQTT client.
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
            //await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
            //    .WithTopic("device/status/+/1/TEMPERATURE")
            //    .Build());
            await mqttClient.StartAsync(options);

            mqttClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected");
            });

            mqttClient.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connected");
            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ ContentType = {e.ApplicationMessage.ContentType}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            // StartAsync returns immediately, as it starts a new thread using Task.Run, 
            // and so the calling thread needs to wait.
            Console.ReadLine();
        }
    }
}
