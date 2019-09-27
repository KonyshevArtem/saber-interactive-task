using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LinkedList.Exceptions;

namespace LinkedList
{
    /// <summary>
    /// This struct holds created node and references to other nodes, that was parsed from stream.
    /// </summary>
    public struct NodeInfo
    {
        public readonly ListNode Node;
        public readonly string Prev;
        public readonly string Next;
        public readonly string Rand;

        public NodeInfo(ListNode node, string data, string prev, string next, string rand)
        {
            Prev = prev;
            node.Data = data;
            Next = next;
            Rand = rand;
            Node = node;
        }
    }

    /// <summary>
    /// This class handles linked list deserialization.
    /// </summary>
    public static class ListDeserializer
    {
        /// <summary>
        /// Deserialize linked list from file stream.
        /// </summary>
        /// <param name="fs">File stream to read from</param>
        /// <param name="list">List to deserialize to</param>
        /// <exception cref="UnexpectedEndException">Thrown when reach end of stream before finished deserializing</exception>
        /// <exception cref="UnexpectedCharException">Thrown when found unexpected char</exception>
        public static void Deserialize(FileStream fs, ListRand list)
        {
            CheckStreamBeginning(fs);

            List<NodeInfo> nodeInfos = new List<NodeInfo>();
            bool readNode = false;
            while (true)
            {
                int b = fs.ReadByte();

                if (b == '{' && !readNode)
                {
                    readNode = true;
                    nodeInfos.Add(DeserializeNode(fs));
                }
                else if (b == ',' && readNode)
                {
                    readNode = false;
                }
                else if (b == -1)
                {
                    throw new UnexpectedEndException();
                }
                else if (b == ']')
                {
                    break;
                }
                else
                {
                    throw new UnexpectedCharException((char) b);
                }
            }

            FillList(list, nodeInfos);
        }

        /// <summary>
        /// Check stream not empty and starts with open bracket.
        /// </summary>
        /// <param name="fs">File stream to check</param>
        /// <exception cref="EmptyStreamException">Thrown if stream is empty or not starts with open bracket</exception>
        private static void CheckStreamBeginning(FileStream fs)
        {
            int firstByte = fs.ReadByte();
            if (firstByte == -1 || firstByte != '[')
            {
                throw new EmptyStreamException();
            }
        }

        /// <summary>
        /// Restore linked list fields and node references.
        /// </summary>
        /// <param name="list">Linked list to restore</param>
        /// <param name="nodeInfos">List of NodeInfo with nodes and their field values</param>
        private static void FillList(ListRand list, List<NodeInfo> nodeInfos)
        {
            if (nodeInfos.Count <= 0) return;
            ConnectNodes(nodeInfos);
            list.Head = nodeInfos.First().Node;
            list.Tail = nodeInfos.Last().Node;
            list.Count = nodeInfos.Count;
        }

        /// <summary>
        /// Restore node references.
        /// </summary>
        /// <param name="nodeInfos">List of NodeInfo with nodes and their field values</param>
        /// <exception cref="UnconnectedNodeException">Thrown when two or more nodes and one of them is not connected to others</exception>
        private static void ConnectNodes(List<NodeInfo> nodeInfos)
        {
            foreach (NodeInfo nodeInfo in nodeInfos)
            {
                if (nodeInfo.Next != "null")
                    nodeInfo.Node.Next = nodeInfos[int.Parse(nodeInfo.Next)].Node;
                if (nodeInfo.Prev != "null")
                    nodeInfo.Node.Prev = nodeInfos[int.Parse(nodeInfo.Prev)].Node;
                if (nodeInfo.Rand != "null")
                    nodeInfo.Node.Rand = nodeInfos[int.Parse(nodeInfo.Rand)].Node;
                if (nodeInfos.Count > 1 && nodeInfo.Node.Next == null && nodeInfo.Node.Prev == null)
                {
                    throw new UnconnectedNodeException();
                }
            }
        }

        /// <summary>
        /// Create node and read 4 fields with values from file stream and warp it into NodeInfo.
        /// </summary>
        /// <param name="fs">File stream to read from</param>
        /// <returns>NodeInfo instance with created node and their field values</returns>
        private static NodeInfo DeserializeNode(FileStream fs)
        {
            ListNode node = new ListNode();
            Dictionary<string, string> fields = new Dictionary<string, string>();
            for (int i = 0; i < 3; ++i)
            {
                ReadField(',', fs, fields);
            }

            ReadField('}', fs, fields);

            return new NodeInfo(node, fields["Data"], fields["Prev"], fields["Next"], fields["Rand"]);
        }

        /// <summary>
        /// Read field name and value from file stream and add to dictionary.
        /// </summary>
        /// <param name="stopChar">Char that separates field-value groups</param>
        /// <param name="fs">File stream to read from</param>
        /// <param name="fields">Dict to write field name and value to</param>
        private static void ReadField(char stopChar, FileStream fs, Dictionary<string, string> fields)
        {
            string line = ReadUntil(fs, stopChar);
            string[] parts = line.Split(new[] {'='}, 2);
            fields.Add(parts[0], parts[1]);
        }

        /// <summary>
        /// Read from file stream until found stop char.
        /// If found apostrophe, then start reading until next apostrophe to get string value.
        /// </summary>
        /// <param name="fs">File stream to read from</param>
        /// <param name="stopChar">Char that stops reading</param>
        /// <returns>String that was read from file stream without stop char</returns>
        /// <exception cref="UnexpectedEndException">Thrown if reach end of stream before stop char</exception>
        private static string ReadUntil(FileStream fs, char stopChar)
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int b = fs.ReadByte();
                if (b == -1)
                {
                    throw new UnexpectedEndException();
                }

                if ((char) b == stopChar)
                {
                    break;
                }

                if (b == '\'')
                {
                    sb.Append(ReadUntil(fs, '\''));
                }
                else
                {
                    sb.Append((char) b);
                }
            }

            return sb.ToString();
        }
    }
}