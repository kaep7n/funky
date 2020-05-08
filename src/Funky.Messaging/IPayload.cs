using System;

namespace Funky.Messaging
{
    public interface IPayload
    {
        object GetData();

        Type GetDataType();
    }
}