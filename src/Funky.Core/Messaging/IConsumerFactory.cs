namespace Funky.Core.Messaging
{
    public interface IConsumerFactory
    {
        IConsumer<T> Create<T>(string topic);
    }
}
