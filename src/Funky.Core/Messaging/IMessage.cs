namespace Funky.Core.Messaging
{
    public interface IMessage
    {
        T GetContent<T>();
    }
}
