using System.Runtime.Serialization;

namespace WebApi.Exceptions
{
    public class SessionNotExistException : Exception
    {
        public SessionNotExistException()
        {
        }

        public SessionNotExistException(string message) : base(message)
        {
        }

        public SessionNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
