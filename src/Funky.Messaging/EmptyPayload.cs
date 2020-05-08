namespace Funky.Messaging
{
    public class EmptyPayload : IPayload
    {
        public object Get() => null;
    }
}
