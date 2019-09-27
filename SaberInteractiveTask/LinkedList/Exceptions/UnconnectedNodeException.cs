using System.Runtime.Serialization;

namespace LinkedList.Exceptions
{
    public class UnconnectedNodeException : SerializationException
    {
        public UnconnectedNodeException() : base("Found unconnected node")
        {
        }
    }
}