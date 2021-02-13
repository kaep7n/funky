using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Funky.Playground.Prototype
{
    public class TimerFunkTrigger<TFunk> : IFunkTrigger
        where TFunk: class, IFunk
    {
        private readonly Timer timer;
        private readonly IServiceProvider serviceProvider;

        public TimerFunkTrigger(double interval, IServiceProvider serviceProvider)
        {
            this.timer = new Timer(interval);
            this.serviceProvider = serviceProvider;
        }

        public ValueTask EnableAsync()
        {
            this.timer.Elapsed += this.Timer_Elapsed;
            this.timer.Start();

            return ValueTask.CompletedTask;
        }

        public ValueTask DisableAsync()
        {
            this.timer.Stop();
            this.timer.Elapsed -= this.Timer_Elapsed;

            return ValueTask.CompletedTask;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                using var scope = this.serviceProvider.CreateScope();

                var funk = scope.ServiceProvider.GetRequiredService<IFunk>();

                await funk.ExecuteAsync(new Noop());
            }
            catch(Exception exception)
            {

            }
        }
    }
}
