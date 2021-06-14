using Microsoft.Extensions.Logging;
using Proto;
using Proto.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Funky.Playground.ProtoActor.Homematic
{
    public class DeviceController : IActor
    {
        private readonly Dictionary<string, PID> devices = new();
        private readonly ILogger<DeviceController> logger;

        public DeviceController(ILogger<DeviceController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started:
                    await this.OnStarted(context);
                    break;
                case DeviceData msg:
                    this.OnDeviceData(context, msg);
                    break;
                default:
                    break;
            }
        }

        private async Task OnStarted(IContext context)
        {
            this.logger.LogInformation("started device controller");
            var httpClient = new HttpClient();

            var response = await httpClient.GetFromJsonAsync<DeviceQueryResult>("http://192.168.2.101:2121/device");

            foreach (var link in response.Links)
            {
                if (link.IsParentRef)
                    continue;

                var props = context.System.DI().PropsFor<Device>();
                var pid = context.Spawn(props);
                this.devices.Add(link.Href, pid);
            }
        }

        private void OnDeviceData(IContext context, DeviceData msg)
        {
            var device = this.devices[msg.Device];
            context.Forward(device);
            this.logger.LogInformation($"forwarded device data to device {device.Id}");
        }
    }
}
