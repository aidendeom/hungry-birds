using System;
using System.Collections.Generic;

namespace hungry_birds
{
    /// <summary>
    /// Class to store BoardConfig in a tree structure to implement minimax algorithm
    /// </summary>
    /// <typeparam name="BoardConfig">Parameter of collection</typeparam>
    class BCTree<BoardConfig>
    {
        private BoardConfig data;
        public LinkedList<BCTree<BoardConfig>> children { get; set; }

        public BCTree(BoardConfig data)
        {
            this.data = data;
            children = new LinkedList<BCTree<BoardConfig>>();
            // count = 1;
        }

        public void AddChild(BoardConfig child)
        {
            children.AddFirst(new BCTree<BoardConfig>(child));
            // count++;
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