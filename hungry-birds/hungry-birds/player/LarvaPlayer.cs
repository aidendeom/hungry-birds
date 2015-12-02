using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hungry_birds
{
    /// <summary>
    /// Class for a player playing the Larva
    /// </summary>
    public class LarvaPlayer : IPlayer
    {
        private Larva _larva = null;

        public LarvaPlayer(Larva larva)
        {
            _larva = larva;
        }

        /// <summary>
        /// Prompts the player for input to create a move.
        /// </summary>
        /// <returns>A move for the player to make</returns>
        public void DoMove()
        {
            // Keep trying to get a valid move
            while (true)
            {
                try
                {
                    var move = GetMove();
                    _larva.Move(move);
                    break;
                }
                catch (InvalidMoveException) { }
            }
        }

        private Move GetMove()
        {
            // Should read a input string. e.g. e1
            string coord = Console.In.ReadLine();
            var to = Position.MakePositionFromCoord(coord);

            return new Move(_larva.Pos, to);
        }

        public void DoAIMove(Larva Larva, Bird[] Birds, Board Board)
        {
            BoardConfig nextConfig = AILarvaMove(Larva, Birds, Board);
            
            try
            {
                _larva.Move(new Move(_larva.Pos, nextConfig.LarvaPos));
            }
            catch (InvalidMoveException) { }
        }

        public BoardConfig AILarvaMove(Larva Larva, Bird[] Birds, Board Board)
        {
            // Get original positions
            Position origLarvaPosition = Larva.Pos;

            Position[] origBirdsPosition = new Position[Birds.Length];

            for (int i = 0; i < Birds.Length; ++i)
            {
                origBirdsPosition[i] = Birds[i].Pos;
            }

            BoardConfig origBC = new BoardConfig(0, origLarvaPosition, origBirdsPosition);

            BCTree<BoardConfig> MiniMaxTree = new BCTree<BoardConfig>(origBC);

            // Get level 1 kids for Larva
            Utilities.generateLarvaChildren(ref MiniMaxTree, 1, Board);

            List<BCTree<BoardConfig>> level1Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 1, ref level1Nodes);
            // Console.WriteLine("Count of nodes at Level 1 given by drill method = " + level1Nodes.Count);

            // Get level 2 kids for the Birds
            for (int i = 0; i < level1Nodes.Count; ++i)
            {
                var temp = level1Nodes[i];
                Utilities.generateBirdsChildren(ref temp, 2, Board);
            }

            List<BCTree<BoardConfig>> level2Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 2, ref level2Nodes);
            // Console.WriteLine("Count of nodes at Level 2 given by drill method = " + level2Nodes.Count);

            // Get level 3 kids for the larva
            for (int i = 0; i < level2Nodes.Count; ++i)
            {
                var temp = level2Nodes[i];
                Utilities.generateLarvaChildren(ref temp, 3, Board);
            }

            List<BCTree<BoardConfig>> level3Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 3, ref level3Nodes);
            // Console.WriteLine("Count of nodes at Level 3 given by drill method = " + level3Nodes.Count);

            // Calculate heuristics
            // Console.WriteLine("Calculating level 3 heuristics...");
            Utilities.calculateLevelHeuristics(ref level3Nodes);

            for (int i = 0; i < level3Nodes.Count; i++)
            {
                // Console.WriteLine(i + " " + level3Nodes[i].data.heuristic);
            }

            // Console.WriteLine("Calculating level 2 heuristics...");
            Utilities.calculateLevelHeuristics(ref level2Nodes);

            for (int i = 0; i < level2Nodes.Count; i++)
            {
                //Console.WriteLine(i + " " + level2Nodes[i].data.heuristic);
            }

            //Console.WriteLine("Calculating level 1 heuristics...");
            Utilities.calculateLevelHeuristics(ref level1Nodes);

            for (int i = 0; i < level1Nodes.Count; i++)
            {
                //Console.WriteLine(i + " " + level1Nodes[i].data.heuristic);
            }

            Console.WriteLine();
            Console.Write("Current Positions - Larva = " + Utilities.GetScoreForPos(MiniMaxTree.data.LarvaPos));
            for (int i = 0; i < MiniMaxTree.data.BirdsPos.Length; ++i)
            {
                Console.Write(", Bird " + (i + 1) + " = " + Utilities.GetScoreForPos(MiniMaxTree.data.BirdsPos[i]));
            }
            Console.WriteLine();
            Console.WriteLine("Calculating best move for Larva...");
            BoardConfig nextConfig = Utilities.getBestMove(level1Nodes, ref MiniMaxTree, true);
            Position nextLarvaPosition = nextConfig.LarvaPos;

            Utilities.PreOrderPrintLarva(MiniMaxTree);
            
            Console.WriteLine("The best next move for the Larva is to go to position " + Utilities.GetScoreForPos(nextLarvaPosition));
            Console.WriteLine();

            return nextConfig;
        }
    }
}
