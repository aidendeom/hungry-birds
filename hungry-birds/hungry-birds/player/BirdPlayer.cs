using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hungry_birds
{
    /// <summary>
    /// Class for player playing the Birds
    /// </summary>
    public class BirdPlayer : IPlayer
    {
        public static readonly char[] SEPERATORS = { ' ' };

        private Bird[] _birds = null;

        public BirdPlayer(Bird[] birds)
        {
            _birds = birds;
        }

        public void DoMove()
        {
            // Keep trying until a valid move is produced
            while (true)
            {
                try
                {
                    var move = GetMove();
                    var bird = GetBirdToMove(move);
                    bird.Move(move);
                    break;
                }
                catch (InvalidMoveException) { }
            }
        }

        private Move GetMove()
        {
            try
            {
                string input = Console.In.ReadLine();
                string[] coords = input.Split(SEPERATORS);

                var from = Position.MakePositionFromCoord(coords[0]);
                var to = Position.MakePositionFromCoord(coords[1]);

                return new Move(from, to);
            }
            catch (Exception) { throw new InvalidMoveException(); }
        }

        private Bird GetBirdToMove(Move move)
        {
            foreach (var bird in _birds)
            {
                if (bird.Pos == move.From)
                    return bird;
            }

            throw new InvalidMoveException();
        }

        public void DoAIMove(Larva Larva, Bird[] Birds, Board Board)
        {
            int birdIndex = 4;
            var to = new Position();

            BoardConfig nextConfig = AIBirdsMove(Larva, Birds, Board);

            for (int i = 0; i < _birds.Length; ++i)
            {
                if (!(nextConfig.BirdsPos[i].Equals(_birds[i].Pos)))
                {
                    birdIndex = i;
                    to = nextConfig.BirdsPos[i];
                }
            }

            try
            {
                _birds[birdIndex].Move(new Move(_birds[birdIndex].Pos, to));
            }
            catch (InvalidMoveException) { }
        }

        public BoardConfig AIBirdsMove(Larva Larva, Bird[] Birds, Board Board)
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

            // Get level 1 kids for the Birds
            Utilities.generateBirdsChildren(ref MiniMaxTree, 1, Board);

            List<BCTree<BoardConfig>> level1Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 1, ref level1Nodes);
            //Console.WriteLine("Count of nodes at Level 1 given by drill method = " + level1Nodes.Count);

            // Get level 2 kids for the Larva
            for (int i = 0; i < level1Nodes.Count; ++i)
            {
                var temp = level1Nodes[i];
                Utilities.generateLarvaChildren(ref temp, 2, Board);
            }

            List<BCTree<BoardConfig>> level2Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 2, ref level2Nodes);
            //Console.WriteLine("Count of nodes at Level 2 given by drill method = " + level2Nodes.Count);

            // Get level 3 kids for the Birds
            for (int i = 0; i < level2Nodes.Count; ++i)
            {
                var temp = level2Nodes[i];
                Utilities.generateBirdsChildren(ref temp, 3, Board);
            }

            List<BCTree<BoardConfig>> level3Nodes = new List<BCTree<BoardConfig>>();
            Utilities.drill(MiniMaxTree, 0, 3, ref level3Nodes);
            //Console.WriteLine("Count of nodes at Level 3 given by drill method = " + level3Nodes.Count);

            //Console.WriteLine("Calculating level 3 heuristics...");
            Utilities.calculateLevelHeuristics(ref level3Nodes);

            for (int i = 0; i < level3Nodes.Count; i++)
            {
                //Console.WriteLine(i + " " + level3Nodes[i].data.heuristic);
            }

            //Console.WriteLine("Calculating level 2 heuristics...");
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
            Console.WriteLine("Calculating best move for Birds...");
            BoardConfig nextConfig = Utilities.getBestMove(level1Nodes, ref MiniMaxTree);
            int birdToMove = getBirdToMove(nextConfig, origBC);
            Position nextBirdPosition = nextConfig.BirdsPos[birdToMove];

            Utilities.PreOrderPrintBirds(MiniMaxTree);
            
            Console.WriteLine("The best next move for the Birds is for Bird " + (birdToMove + 1) + " to go to position " + Utilities.GetScoreForPos(nextBirdPosition));
            Console.WriteLine();

            return nextConfig;
        }

        private int getBirdToMove(BoardConfig nextConfig, BoardConfig origBC)
        {
            for (int i = 0; i < nextConfig.BirdsPos.Length; ++i)
            {
                if (!(origBC.BirdsPos[i].Equals(nextConfig.BirdsPos[i])))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}