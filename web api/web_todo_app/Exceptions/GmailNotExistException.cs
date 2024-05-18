using System.Runtime.Serialization;

namespace web_todo_app.Exceptions
{
    public class GmailNotExistException : Exception
    {
        public GmailNotExistException()
        {
        }

        public GmailNotExistException(string message) : base(message)
        {
        }

        public GmailNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GmailNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
