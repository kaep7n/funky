using Proto;
using Proto.Router;
using System.Drawing;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Funky.Playground.ProtoActor.Homematic
{
    public class PersistorGroup : IActor
    {
        public PersistorGroup()
        {
            Console.WriteLine("persistor group created", Color.PaleVioletRed);
        }

        public Task ReceiveAsync(IContext context)
        {
            if (context.Message is Started)
            {
                Console.WriteLine("persistor group started", Color.PaleVioletRed);

                var persistorProps = Props.FromProducer(() => new Persistor());
                var poolProps = context.NewRoundRobinPool(persistorProps, 5);
                var pid = context.Spawn(poolProps);

                Console.WriteLine("persistors spawned", Color.PaleVioletRed);

                context.System.EventStream.Subscribe<DeviceData>(msg =>
                {
                    context.Send(pid, msg);
                });
            }

            return Task.CompletedTask;
        }
    }

    public class Persistor : IActor
    {
        public Persistor()
        {
            Console.WriteLine("persistor created", Color.Coral);
        }

        public Task ReceiveAsync(IContext context)
        {
            if(context.Message is Started)
            {
                Console.WriteLine("persistor started", Color.Coral);
            }
            if (context.Message is DeviceData msg)
            {
                Console.WriteLine($"persisting message {msg}", Color.Coral);
            }

            return Task.CompletedTask;
        }
    }
}
