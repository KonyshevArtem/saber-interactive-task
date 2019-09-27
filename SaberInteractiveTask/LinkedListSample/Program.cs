using System;
using System.IO;
using LinkedList;

namespace LinkedListSample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SerializeSample();
            DeserializeSample();
        }

        private static void DeserializeSample()
        {
            ListRand list = new ListRand();
            using (FileStream fs = new FileStream("test.json", FileMode.Open))
            {
                list.Deserialize(fs);
            }

            Console.WriteLine(list.Head.Data);
            Console.WriteLine(list.Head.Next.Data);
        }

        private static void SerializeSample()
        {
            ListRand list = new ListRand();
            ListNode head = new ListNode {Data = "test1"};
            ListNode tail = new ListNode {Data = "test2", Prev = head};
            list.Head = head;
            list.Tail = tail;
            head.Next = tail;
            head.Rand = tail;
            tail.Rand = tail;
            using (FileStream fs = new FileStream("test.json", FileMode.OpenOrCreate))
            {
                list.Serialize(fs);
            }
        }
    }
}