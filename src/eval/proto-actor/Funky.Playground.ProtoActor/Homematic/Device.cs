using Microsoft.Extensions.Logging;
using Proto;
using System;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor.Homematic
{
    public class Device : IActor
    {
        private readonly string identifier;
        private readonly ILogger<Device> logger;
        private DeviceInformation information;

        public Device(ILogger<Device> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started:
                    await this.OnStarted();
                    break;
                case DeviceData:
                    this.OnDeviceData(context);
                    break;
                default:
                    break;
            }
        }

        private async Task OnStarted()
        {
            //var httpClient = new HttpClient();

            //this.information = await httpClient.GetFromJsonAsync<DeviceInformation>($"http://192.168.2.101:2121/device/{this.identifier}");
        }

        private void OnDeviceData(IContext context)
            => context.System.EventStream.Publish(context.Message);
    }
}
