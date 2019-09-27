using System.Runtime.Serialization;

namespace LinkedList.Exceptions
{
    public class UnexpectedEndException : SerializationException
    {
        public UnexpectedEndException() : base("Unexpected end of stream")
        {
        }
    }
}