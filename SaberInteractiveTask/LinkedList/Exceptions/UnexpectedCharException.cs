using System.Runtime.Serialization;

namespace LinkedList.Exceptions
{
    public class UnexpectedCharException : SerializationException
    {
        public UnexpectedCharException(char c) : base($"Unexpected char: \'{c}\'''")
        {
        }
    }
}