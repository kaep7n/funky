using System;
using System.Runtime.Serialization;

namespace Funky.Core
{
    [Serializable]
    public class InvalidStartupException : Exception
    {
        public InvalidStartupException()
        {
        }

        public InvalidStartupException(string message) : base(message)
        {
        }

        public InvalidStartupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStartupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}