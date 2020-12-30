namespace Funky.Core.Messaging
{
    public interface IConsumerFactory
    {
        IConsumer Create(string topic);
    }
}
