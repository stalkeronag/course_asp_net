using System.Runtime.Serialization;

namespace web_todo_app.Exceptions
{
    public class WrongPasswordUserException : Exception
    {
        public WrongPasswordUserException()
        {
        }

        public WrongPasswordUserException(string message) : base(message)
        {
        }

        public WrongPasswordUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongPasswordUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
