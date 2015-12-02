using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hungry_birds
{
    public static class Utilities
    {

        public static int GetScoreForPos(Position pos)
        {
            /*
 The board's score looks like the following

 1	2	3	4	4	3	2	1
 2	4	6	8	8	6	4	2
 3	6	9	12	12	9	6	3
 4	8	12	16	16	12	8	4
 5	10	15	20	20	15	10	5
 6	12	18	24	24	18	12	6
 7	14	21	28	28	21	14	7
 8	16	24	32	32	24	16	8
 */


            int rowScore = pos.Row + 1;

            int dist1 = Math.Abs(pos.Col - 3);
            int dist2 = Math.Abs(pos.Col - 4);
            int distFromCenter = Math.Min(dist1, dist2);
            int colScore = 4 - distFromCenter;

            return rowScore * colScore;
        }

        // get all nodes on a level
        // reference - http://stackoverflow.com/questions/13349853/find-all-nodes-in-a-binary-tree-on-a-specific-level-interview-query
        public static void drill(BCTree<BoardConfig> node, int cLevel, int rLevel, ref List<BCTree<BoardConfig>> result)
        {
            if (cLevel == rLevel)
                result.Add(node);
            else
            {
                foreach (BCTree<BoardConfig> b in node.children)
                {
                    drill(b, cLevel + 1, rLevel, ref result);
                }
            }
        }

        public static void calculateLevelHeuristics(ref List<BCTree<BoardConfig>> lNodes)
        {
            for (int i = 0; i < lNodes.Count; ++i)
            {
                if ((lNodes[i].Level % 2) == 0) // Calculate heuristics of level 2 or level 0, so MAX of children
                {
                    if (lNodes[i].isLeaf)
                    {
                        BoardConfig tempBC = new BoardConfig(lNodes[i].data);
                        int tempHeuristic = lNodes[i].data.EvaluateBCHeuristic();
                        tempBC.heuristic = tempHeuristic;
                        lNodes[i].data = tempBC;
                    }
                    else
                    {
                        BoardConfig tempBC = new BoardConfig(lNodes[i].data);
                        int MAXHeuristic = lNodes[i].children[0].data.heuristic;
                        for (int j = 1; j < lNodes[i].children.Count; ++j)
                        {
                            if (lNodes[i].children[j].data.heuristic > MAXHeuristic)
                                MAXHeuristic = lNodes[i].children[j].data.heuristic;
                        }
                        tempBC.heuristic = MAXHeuristic;
                        lNodes[i].data = tempBC;
                    }
                }
                else if ((lNodes[i].Level % 2) == 1) // Calculate heuristics of level 3 or level 1, so MIN of children (or just calculate heuristic for level 3)
                {
                    if (lNodes[i].isLeaf)
                    {
                        BoardConfig tempBC = new BoardConfig(lNodes[i].data);
                        int tempHeuristic = lNodes[i].data.EvaluateBCHeuristic();
                        tempBC.heuristic = tempHeuristic;
                        lNodes[i].data = tempBC;
                    }
                    else
                    {
                        BoardConfig tempBC = new BoardConfig(lNodes[i].data);
                        int MINHeuristic = lNodes[i].children[0].data.heuristic;
                        for (int j = 1; j < lNodes[i].children.Count; ++j)
                        {
                            if (lNodes[i].children[j].data.heuristic < MINHeuristic)
                                MINHeuristic = lNodes[i].children[j].data.heuristic;
                        }
                        tempBC.heuristic = MINHeuristic;
                        lNodes[i].data = tempBC;
                    }
                }
            }
        }

        public static BoardConfig getBestMove(List<BCTree<BoardConfig>> level1Nodes, ref BCTree<BoardConfig> rootNode, bool larva)
        {
            // Calculate MAX of children and place it in root, as well as return Board Configuration of MAX child
            if (rootNode.isLeaf)
            {
                BoardConfig tempBC = new BoardConfig(rootNode.data);
                int tempHeuristic = rootNode.data.EvaluateBCHeuristic();
                tempBC.heuristic = tempHeuristic;
                rootNode.data = tempBC;
                //Console.WriteLine("Root is a leaf");
                return rootNode.data;
            }
            else
            {
                BoardConfig tempBC = new BoardConfig(rootNode.data);
                int MAXHeuristic = rootNode.children[0].data.heuristic;
                BoardConfig MAXConfig = rootNode.children[0].data;
                for (int i = 1; i < rootNode.children.Count; ++i)
                {
                    if (larva)
                    {
                        if (rootNode.children[i].data.heuristic > MAXHeuristic)
                        {
                            MAXHeuristic = rootNode.children[i].data.heuristic;
                            MAXConfig = rootNode.children[i].data;
                        }
                    }
                    else
                    {
                        if (rootNode.children[i].data.heuristic < MAXHeuristic)
                        {
                            MAXHeuristic = rootNode.children[i].data.heuristic;
                            MAXConfig = rootNode.children[i].data;
                        }
                    }
                }
                rootNode.data = MAXConfig;
                return MAXConfig;
            }
        }

        // TODO make method less repetitive
        public static void generateLarvaChildren(ref BCTree<BoardConfig> parentNode, int lvl, Board Board)
        {
            //Console.Write("ParentNode Larva = " + GetScoreForPos(parentNode.data.LarvaPos));
            for (int i = 0; i < parentNode.data.BirdsPos.Length; ++i)
            {
                //Console.Write(", Bird " + (i + 1) + " = " + GetScoreForPos(parentNode.data.BirdsPos[i]));
            }
            //Console.WriteLine();

            Position topLeftPosition = new Position(parentNode.data.LarvaPos.Row - 1, parentNode.data.LarvaPos.Col - 1);
            if (Board.IsValidPosition(topLeftPosition) && parentNode.data.IsCellEmpty(topLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = topLeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Top left position = " + GetScoreForPos(topLeftPosition));
            }

            Position topRightPosition = new Position(parentNode.data.LarvaPos.Row - 1, parentNode.data.LarvaPos.Col + 1);
            if (Board.IsValidPosition(topRightPosition) && parentNode.data.IsCellEmpty(topRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = topRightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Top right position = " + GetScoreForPos(topRightPosition));
            }

            Position bottomLeftPosition = new Position(parentNode.data.LarvaPos.Row + 1, parentNode.data.LarvaPos.Col - 1);
            if (Board.IsValidPosition(bottomLeftPosition) && parentNode.data.IsCellEmpty(bottomLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = bottomLeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bottom left position = " + GetScoreForPos(bottomLeftPosition));
            }

            Position bottomRightPosition = new Position(parentNode.data.LarvaPos.Row + 1, parentNode.data.LarvaPos.Col + 1);
            if (Board.IsValidPosition(bottomRightPosition) && parentNode.data.IsCellEmpty(bottomRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = bottomRightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bottom right position = " + GetScoreForPos(bottomRightPosition));
            }
        }

        // TODO make method less repetitive
        public static void generateBirdsChildren(ref BCTree<BoardConfig> parentNode, int lvl, Board Board)
        {
            //Console.Write("ParentNode Larva = " + GetScoreForPos(parentNode.data.LarvaPos));
            for (int i = 0; i < parentNode.data.BirdsPos.Length; ++i)
            {
                //Console.Write(", Bird " + (i + 1) + " = " + GetScoreForPos(parentNode.data.BirdsPos[i]));
            }
            //Console.WriteLine();

            Position bird1LeftPosition = new Position(parentNode.data.BirdsPos[0].Row - 1, parentNode.data.BirdsPos[0].Col - 1);
            if (Board.IsValidPosition(bird1LeftPosition) && parentNode.data.IsCellEmpty(bird1LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[0] = bird1LeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 1 left position = " + GetScoreForPos(bird1LeftPosition));
            }

            Position bird1RightPosition = new Position(parentNode.data.BirdsPos[0].Row - 1, parentNode.data.BirdsPos[0].Col + 1);
            if (Board.IsValidPosition(bird1RightPosition) && parentNode.data.IsCellEmpty(bird1RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[0] = bird1RightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 1 right position = " + GetScoreForPos(bird1RightPosition));
            }

            Position bird2LeftPosition = new Position(parentNode.data.BirdsPos[1].Row - 1, parentNode.data.BirdsPos[1].Col - 1);
            if (Board.IsValidPosition(bird2LeftPosition) && parentNode.data.IsCellEmpty(bird2LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[1] = bird2LeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 2 left position = " + GetScoreForPos(bird2LeftPosition));
            }

            Position bird2RightPosition = new Position(parentNode.data.BirdsPos[1].Row - 1, parentNode.data.BirdsPos[1].Col + 1);
            if (Board.IsValidPosition(bird2RightPosition) && parentNode.data.IsCellEmpty(bird2RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[1] = bird2RightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 2 right position = " + GetScoreForPos(bird2RightPosition));
            }

            Position bird3LeftPosition = new Position(parentNode.data.BirdsPos[2].Row - 1, parentNode.data.BirdsPos[2].Col - 1);
            if (Board.IsValidPosition(bird3LeftPosition) && parentNode.data.IsCellEmpty(bird3LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[2] = bird3LeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 3 left position = " + GetScoreForPos(bird3LeftPosition));
            }

            Position bird3RightPosition = new Position(parentNode.data.BirdsPos[2].Row - 1, parentNode.data.BirdsPos[2].Col + 1);
            if (Board.IsValidPosition(bird3RightPosition) && parentNode.data.IsCellEmpty(bird3RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[2] = bird3RightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 3 right position = " + GetScoreForPos(bird3RightPosition));
            }

            Position bird4LeftPosition = new Position(parentNode.data.BirdsPos[3].Row - 1, parentNode.data.BirdsPos[3].Col - 1);
            if (Board.IsValidPosition(bird4LeftPosition) && parentNode.data.IsCellEmpty(bird4LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[3] = bird4LeftPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 4 left position = " + GetScoreForPos(bird4LeftPosition));
            }

            Position bird4RightPosition = new Position(parentNode.data.BirdsPos[3].Row - 1, parentNode.data.BirdsPos[3].Col + 1);
            if (Board.IsValidPosition(bird4RightPosition) && parentNode.data.IsCellEmpty(bird4RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.BirdsPos[3] = bird4RightPosition;
                parentNode.AddChild(tempBC);
                //Console.WriteLine("Bird 4 right position = " + GetScoreForPos(bird4RightPosition));
            }
        }

        public static void PreOrderPrintLarva (BCTree<BoardConfig> bct)
        {
            for (int i = 0; i < bct.Level; ++i)
            {
                Console.Write("         ");
            }

            Console.WriteLine("[" + GetScoreForPos(bct.data.LarvaPos) + "] " + bct.data.heuristic);
            
            foreach (BCTree<BoardConfig> b in bct.children)
            {
                PreOrderPrintLarva(b);
            }
        }

        public static void PreOrderPrintBirds(BCTree<BoardConfig> bct)
        {
            for (int i = 0; i < bct.Level; ++i)
            {
                Console.Write("                   ");
            }
            Console.Write("[");
            for (int i = 0; i < bct.data.BirdsPos.Length; ++i)
            {
                Console.Write(GetScoreForPos(bct.data.BirdsPos[i]) + " ");
            }
            Console.WriteLine("] " + bct.data.heuristic);

            foreach (BCTree<BoardConfig> b in bct.children)
            {
                PreOrderPrintBirds(b);
            }
        }

    }
}
