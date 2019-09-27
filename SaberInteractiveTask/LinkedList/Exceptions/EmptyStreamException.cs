using System.Runtime.Serialization;

namespace LinkedList.Exceptions
{
    public class EmptyStreamException : SerializationException
    {
        public EmptyStreamException() : base("Stream is empty")
        {
        }
    }
}