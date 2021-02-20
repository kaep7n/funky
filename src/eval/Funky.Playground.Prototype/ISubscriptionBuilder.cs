namespace Funky.Playground.Prototype
{
    public interface ISubscriptionBuilder
    {
        ISubscriptionBuilder Timer(double interval);

        ISubscriptionBuilder Topic<TMessage>(string topic);
    }
}
