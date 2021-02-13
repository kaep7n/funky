using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Funky.Playground.Prototype
{
    public class TimerSubscription : ISubscription
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Type targetType;
        private readonly Timer timer;

        public TimerSubscription(IServiceScopeFactory serviceScopeFactory, Type targetType, double interval)
        {
            this.timer = new Timer(interval);
            this.serviceScopeFactory = serviceScopeFactory;
            this.targetType = targetType;
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
            using var scope = this.serviceScopeFactory.CreateScope();

            if (scope.ServiceProvider.GetService(this.targetType) is not IFunk<TimerFired> funk)
                return;

            await funk.ExecuteAsync(new TimerFired(DateTimeOffset.UtcNow));
        }
    }
}
