using System.IO;
using LinkedList;

namespace LinkedListSample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ListRand list = new ListRand();
            ListNode node1 = new ListNode {Data = "test1"};
            ListNode node2 = new ListNode {Data = "test2", Prev = node1};
            list.Head = node1;
            list.Tail = node2;
            node1.Next = node2;
            node1.Rand = node2;
            node2.Rand = node2;
            using (FileStream fs = new FileStream("test.json", FileMode.OpenOrCreate))
            {
                list.Serialize(fs);
            }
        }
    }
}