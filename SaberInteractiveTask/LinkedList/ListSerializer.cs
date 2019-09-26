using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinkedList
{
    /// <summary>
    /// This class handles linked list serialization.
    /// </summary>
    public class ListSerializer
    {
        /// <summary>
        /// Serialize linked list and write to file stream.
        /// </summary>
        /// <param name="fs">File stream for writing serialized list</param>
        /// <param name="list">Linked list to serialize</param>
        public static void Serialize(FileStream fs, ListRand list)
        {
            Dictionary<ListNode, int> nodeToIndex = GetNodeToIndex(list);
            string listJson = GetListJson(list, nodeToIndex);
            byte[] bytes = new UTF8Encoding(true).GetBytes(listJson);
            fs.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Write list with all nodes to string in json format.
        /// Stop writing json when next node is null or hit cycle.
        /// </summary>
        /// <param name="list">List to write to json</param>
        /// <param name="nodeToIndex">Dictionary with nodes paired with their indexes</param>
        /// <returns>List in json format</returns>
        private static string GetListJson(ListRand list, Dictionary<ListNode, int> nodeToIndex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            HashSet<ListNode> visitedNodes = new HashSet<ListNode>();
            ListNode node = list.Head;
            while (node != null)
            {
                visitedNodes.Add(node);
                WriteNode(node, sb, nodeToIndex);
                node = node.Next;
                if (node == null || visitedNodes.Contains(node))
                {
                    break;
                }

                sb.Append(",");
            }

            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Write node's field values to string builder.
        /// References to prev, next and rand nodes written as indexes of corresponding node.
        /// </summary>
        /// <param name="node">Node which field values are written to string builder</param>
        /// <param name="sb">String builder to write to</param>
        /// <param name="nodeToIndex">Dictionary with nodes paired with their indexes</param>
        private static void WriteNode(ListNode node, StringBuilder sb, Dictionary<ListNode, int> nodeToIndex)
        {
            sb.Append("{");
            sb.Append($"Data='{node.Data}',");
            sb.Append($"Prev={(node.Prev == null ? "null" : nodeToIndex[node.Prev].ToString())},");
            sb.Append($"Next={(node.Next == null ? "null" : nodeToIndex[node.Next].ToString())},");
            sb.Append($"Rand={(node.Rand == null ? "null" : nodeToIndex[node.Rand].ToString())}");
            sb.Append("}");
        }

        /// <summary>
        /// Create a dictionary where each node is paired with it's index, if it was a simple array.
        /// Stop creating dictionary when next node is null or hit cycle.
        /// </summary>
        /// <param name="list">Linked list</param>
        /// <returns>Dictionary with nodes paired with their indexes</returns>
        private static Dictionary<ListNode, int> GetNodeToIndex(ListRand list)
        {
            Dictionary<ListNode, int> nodeToIndex = new Dictionary<ListNode, int>();
            ListNode node = list.Head;
            int index = 0;
            while (node != null && !nodeToIndex.ContainsKey(node))
            {
                nodeToIndex.Add(node, index++);
                node = node.Next;
            }

            return nodeToIndex;
        }
    }
}