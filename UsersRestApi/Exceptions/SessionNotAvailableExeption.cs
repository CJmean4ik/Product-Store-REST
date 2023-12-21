using System.Runtime.Serialization;

namespace ProductAPI
{
    [Serializable]
    internal class SessionNotAvailableExeption : Exception
    {
        public SessionNotAvailableExeption() 
            : base("The session for this user has expired. It is not possible to receive the product for the shopping cart")
        {

        }

        public SessionNotAvailableExeption(string? message) : base(message)
        {
        }

        public SessionNotAvailableExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SessionNotAvailableExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}