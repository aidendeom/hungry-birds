using System;
using System.Collections.Generic;

namespace hungry_birds
{
    /// <summary>
    /// Class to store BoardConfig in a tree structure to implement minimax algorithm
    /// </summary>
    /// <typeparam name="BoardConfig">Parameter of collection</typeparam>
    public class BCTree<BoardConfig>
    {
        public BoardConfig data { get; set; }
        public BCTree<BoardConfig> parent { get; set; }
        public List<BCTree<BoardConfig>> children { get; set; }

        public Boolean isRoot
        {
            get {return parent == null;}
        }

        public Boolean isLeaf
        {
            get { return children.Count == 0; }
        }

        public int Level
        {
            get
            {
                if (this.isRoot)
                    return 0;
                return parent.Level + 1;
            }
        }

        public BCTree(BoardConfig data)
        {
            this.data = data;
            children = new List<BCTree<BoardConfig>>();
            
            // count = 1;
        }

        public BCTree<BoardConfig> AddChild(BoardConfig child)
        {
            BCTree<BoardConfig> childNode = new BCTree<BoardConfig>(child) { parent = this };
            this.children.Add(childNode);
            // children.AddFirst(new BCTree<BoardConfig>(child));
            // count++;

            return childNode;
        }

        public BCTree<BoardConfig> GetChild(int i)
        {
            foreach (BCTree<BoardConfig> n in children)
            {
                if (--i == 0)
                {
                    return n;
                }
            }
            return null;
        }

        public BoardConfig GetData()
        {
            return data;
        }
    }
}
// reference - http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp
// reference - https://code.google.com/p/yet-another-tree-structure/