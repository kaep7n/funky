namespace Funky.Core.Events
{
    public interface IConsumerFactory
    {
        IConsumer<T> Create<T>(string topic);
    }
}
