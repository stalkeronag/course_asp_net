using System.Runtime.Serialization;

namespace WebApi.Exceptions
{
    public class RefreshSessionBuilderException : Exception
    {
        public RefreshSessionBuilderException()
        {
        }

        public RefreshSessionBuilderException(string message) : base(message)
        {
        }

        public RefreshSessionBuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RefreshSessionBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
