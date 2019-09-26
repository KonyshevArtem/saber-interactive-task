using System.IO;
using System.Text;
using LinkedList;
using NUnit.Framework;


namespace LinkedListTests
{
    [TestFixture]
    public class SerializeTests
    {
        private FakeFileStream fileStream;

        private void AssertFileStream(string expected)
        {
            byte[] actualBytes = new byte[expected.Length];
            fileStream.Read(actualBytes, 0, 0);
            string actual = new UTF8Encoding(true).GetString(actualBytes);
            Assert.AreEqual(expected, actual);
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            fileStream = new FakeFileStream("test.json", FileMode.OpenOrCreate);
        }

        [Test]
        public void ShouldSerialize_EmptyList()
        {
            ListRand list = new ListRand();
            list.Serialize(fileStream);
            AssertFileStream("[]");
        }

        [Test]
        public void ShouldSerialize_List_WhenOneNode_WithoutRandNode()
        {
            ListNode node = new ListNode {Data = "sobaka"};
            ListRand list = new ListRand {Head = node, Tail = node};
            list.Serialize(fileStream);
            AssertFileStream("[{Data='sobaka',Prev=null,Next=null,Rand=null}]");
        }

        [Test]
        public void ShouldSerialize_List_WhenSeveralNodes_WithoutRandNode()
        {
            ListNode node1 = new ListNode {Data = "1"};
            ListNode node2 = new ListNode {Data = "2", Prev = node1};
            node1.Next = node2;
            ListRand list = new ListRand {Head = node1, Tail = node2};
            list.Serialize(fileStream);
            AssertFileStream("[{Data='1',Prev=null,Next=1,Rand=null},{Data='2',Prev=0,Next=null,Rand=null}]");
        }

        [Test]
        public void ShouldSerialize_List_WhenOneNode_WithRand()
        {
            ListNode node = new ListNode {Data = "sobaka"};
            node.Rand = node;
            ListRand list = new ListRand {Head = node, Tail = node};
            list.Serialize(fileStream);
            AssertFileStream("[{Data='sobaka',Prev=null,Next=null,Rand=0}]");
        }

        [Test]
        public void ShouldSerialize_List_WhenSeveralNodes_WithRandNode()
        {
            ListNode node1 = new ListNode {Data = "1"};
            ListNode node2 = new ListNode {Data = "2", Prev = node1, Rand = node1};
            node1.Rand = node1;
            node1.Next = node2;
            ListRand list = new ListRand {Head = node1, Tail = node2};
            list.Serialize(fileStream);
            AssertFileStream("[{Data='1',Prev=null,Next=1,Rand=0},{Data='2',Prev=0,Next=null,Rand=0}]");
        }

        [Test]
        public void ShouldSerialize_List_WithCycle()
        {
            ListNode node1 = new ListNode {Data = "1"};
            ListNode node2 = new ListNode {Data = "2", Prev = node1, Rand = node1};
            ListNode node3 = new ListNode {Data = "3", Prev = node2, Rand = node1, Next = node1};
            node1.Rand = node3;
            node1.Next = node2;
            node1.Prev = node3;
            node2.Next = node3;
            ListRand list = new ListRand {Head = node1, Tail = node3};
            list.Serialize(fileStream);
            AssertFileStream("[{Data='1',Prev=2,Next=1,Rand=2},{Data='2',Prev=0,Next=2,Rand=0},{Data='3',Prev=1,Next=0,Rand=0}]");
        }
    }
}