using System.IO;

namespace LinkedList
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            ListSerializer.Serialize(s, this);
        }

        public void Deserialize(FileStream s)
        {
        }
    }
}