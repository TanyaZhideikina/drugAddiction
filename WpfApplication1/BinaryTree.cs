using System;
using System.Drawing;

namespace WpfApplication1
{
    public class BinaryTree
    {
        /// <summary>
        /// The root node, it won't be seen on the graph!
        /// </summary>
        public Node RootNode { get; set; }

        public BinaryTree()
        {
            RootNode = new Node(int.MinValue);
        }

        /// <summary>
        /// Get the count of nodes on the tree
        /// </summary>
        public int Count { get { return RootNode.Right.Count; } }

        /// <summary>
        /// Add a node to the tree
        /// </summary>
        public void Add(Node node, bool addLeft)
        {
            RootNode.Add(node, addLeft);
        }
        
        /// <summary>
        /// Draw the tree
        /// </summary>
        /// <returns>Image object</returns>
        public Image Draw()
        {
            GC.Collect();// collects the unreffered locations on the memory
            int temp;
            return RootNode.Right == null ? null : RootNode.Right.Draw(out temp);
        }

    }
}
