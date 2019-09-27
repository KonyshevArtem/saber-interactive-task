using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LinkedList;
using LinkedList.Exceptions;
using NUnit.Framework;

namespace LinkedListTests
{
    [TestFixture]
    public class DeserializeTests
    {
        private FakeFileStream fileStream;

        private void WriteToFileStream(string value)
        {
            byte[] bytes = new UTF8Encoding(true).GetBytes(value);
            fileStream.Write(bytes, 0, 0);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fileStream = new FakeFileStream("deserialize_test.json", FileMode.OpenOrCreate);
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenEmptyStream()
        {
            ListRand list = new ListRand();
            WriteToFileStream("");
            Assert.Throws<EmptyStreamException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenNoOpenBracket()
        {
            ListRand list = new ListRand();
            WriteToFileStream("{}");
            Assert.Throws<EmptyStreamException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void ShouldDeserialize_WhenEmptyList()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[]");
            list.Deserialize(fileStream);
            Assert.Null(list.Head);
            Assert.Null(list.Tail);
            Assert.Zero(list.Count);
        }

        [Test]
        public void ShouldDeserialize_WhenOneNode_WithNoRandNode()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=null,Rand=null}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.NotNull(list.Tail);
            Assert.AreSame(list.Head, list.Tail);
            Assert.AreEqual(1, list.Count);
        }

        [Test]
        public void ShouldDeserialize_WhenSeveralNodes_WithNoRandNode()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=1,Rand=null},{Data='',Prev=0,Next=null,Rand=null}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.NotNull(list.Tail);
            Assert.AreNotSame(list.Head, list.Tail);
            Assert.AreEqual(2, list.Count);
            Assert.AreSame(list.Head.Next, list.Tail);
            Assert.AreSame(list.Tail.Prev, list.Head);
        }

        [Test]
        public void ShouldDeserialize_WhenOneNode_WithRand()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=null,Rand=0}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.AreSame(list.Head, list.Head.Rand);
        }

        [Test]
        public void ShouldDeserialize_WhenSeveralNodes_WithRandNode()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=1,Rand=0},{Data='',Prev=0,Next=null,Rand=0}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.AreSame(list.Head, list.Head.Rand);
            Assert.NotNull(list.Tail);
            Assert.AreSame(list.Head, list.Tail.Rand);
        }

        [Test]
        public void ShouldDeserialize_WhenCycle()
        {
            ListRand list = new ListRand();
            WriteToFileStream(
                "[{Data='',Prev=2,Next=1,Rand=null},{Data='',Prev=0,Next=2,Rand=null},{Data='',Prev=1,Next=0,Rand=null}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.NotNull(list.Tail);
            Assert.AreSame(list.Head, list.Tail.Next);
            Assert.AreSame(list.Tail, list.Head.Prev);
        }

        [TestCase("sobaka"),
         TestCase("sob,aka"),
         TestCase("sob{}aka"),
         TestCase("sob[]aka"),
         TestCase("sob=aka")]
        public void ShouldDeserialize_NodeData(string data)
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='" + data + "',Prev=null,Next=null,Rand=null}]");
            list.Deserialize(fileStream);
            Assert.NotNull(list.Head);
            Assert.AreEqual(data, list.Head.Data);
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenFoundUnconnectedNode()
        {
            ListRand list = new ListRand();
            WriteToFileStream(
                "[{Data='',Prev=null,Next=2,Rand=null},{Data='',Prev=null,Next=null,Rand=null},{Data='',Prev=0,Next=null,Rand=null}]");
            Assert.Throws<UnconnectedNodeException>(() => { list.Deserialize(fileStream); });
        }

        [TestCase("["),
         TestCase("[{Data='',Prev=null,Next=null,Rand=0}")]
        public void Deserialize_ShouldThrowException_WhenNoTrailingBracket(string input)
        {
            ListRand list = new ListRand();
            WriteToFileStream(input);
            Assert.Throws<UnexpectedEndException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldNotThrowException_WhenTrailingCommasBetweenNodes()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=null,Rand=0},]");
            Assert.DoesNotThrow(() => { list.Deserialize(fileStream); });
        }

        [TestCase("[{Data='',Prev=null,Next=null,Rand=0,}]"),
         TestCase("[{Data='',Prev=null,Next=null,Rand=aa}]"),
         TestCase("[{Data='',Prev=null,Next=null,Rand=aa,Test=null}]")]
        public void Deserialize_ShouldThrowException_WhenIncorrectReference(string input)
        {
            ListRand list = new ListRand();
            WriteToFileStream(input);
            Assert.Throws<FormatException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenNoField()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=null}]");
            Assert.Throws<UnexpectedEndException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenNoCommaBetweenFields()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=nullNext=null,Rand=null}]");
            Assert.Throws<UnexpectedEndException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenNoCommaBetweenNodes()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Data='',Prev=null,Next=1,Rand=null}{Data='',Prev=0,Next=null,Rand=null}]");
            Assert.Throws<UnexpectedCharException>(() => { list.Deserialize(fileStream); });
        }

        [Test]
        public void Deserialize_ShouldThrowException_WhenWrongFieldName()
        {
            ListRand list = new ListRand();
            WriteToFileStream("[{Dota='',Prev=null,Next=1,Rand=null}]");
            Assert.Throws<KeyNotFoundException>(() => { list.Deserialize(fileStream); });
        }
    }
}