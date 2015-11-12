using System;
using System.Text;
using System.Collections.Generic;

namespace hungry_birds
{
    /// <summary>
    /// Class to represent a board in the game of Hungry Birds
    /// </summary>
    public class Board
    {
        public const int NUM_ROWS = 8;
        public const int NUM_COLS = 8;
        public const int NUM_CELLS = NUM_ROWS * NUM_COLS;
        public const int NUM_BIRDS = 4;

        public static readonly Position INITIAL_LARVA_POS = new Position(6, 3);
        public static readonly Position[] INITIAL_BIRD_POS =
        {
            new Position(7, 0),
            new Position(7, 2),
            new Position(7, 4),
            new Position(7, 6)
        };

        readonly string _rowSeperator;
        readonly string _headerString;

        GamePiece[] _data = new GamePiece[NUM_ROWS * NUM_COLS];

        public Larva Larva { get; private set; }
        public Bird[] Birds { get; private set; }

        /// <summary>
        /// Create an empty board
        /// </summary>
        public Board()
        {
            EmptyBoard();
            _rowSeperator = GetRowSeperator();
            _headerString = GetHeaderString();

            Larva = new Larva(INITIAL_LARVA_POS, this);
            SetCell(Larva.Pos, Larva);

            Birds = new Bird[NUM_BIRDS];
            for (int i = 0; i < NUM_BIRDS; i++)
            {
                Position pos = INITIAL_BIRD_POS[i];
                Birds[i] = new Bird(pos, this);
                SetCell(pos, Birds[i]);
            }
        }

        public void SetCell(Position pos, GamePiece piece)
        {
            if (!IsPositionInMap(pos))
                return;

            _data[pos.Row * NUM_COLS + pos.Col] = piece;
        }

        public GamePiece GetCell(Position pos)
        {
            if (!IsPositionInMap(pos))
                return null;

            return _data[pos.Row * NUM_COLS + pos.Col];
        }

        public bool IsCellEmpty(Position pos)
        {
            if (!IsPositionInMap(pos))
                throw new ArgumentException();

            return GetCell(pos) == null;
        }

        public bool IsValidPosition(Position pos)
        {
            return IsPositionInMap(pos);
        }

        public void Move(Move m)
        {
            var cell = GetCell(m.From);
            SetCell(m.From, null);
            SetCell(m.To, cell);
        }

        /// <summary>
        /// Check the state of the board - Is there a winner?
        /// </summary>
        /// <returns>
        ///     LarvaWin if larva is in winning row
        ///     BirdWin if the birds have eaten the larva
        ///     Running if no winner
        /// </returns>
        public GameState CheckForWinner()
        {
            var birdsCanMove = false;

            // Check if any bird has captured the larva
            foreach (var bird in Birds)
            {
                // Also check that at least one bird can move
                birdsCanMove |= bird.CanMove();

                if (bird.Pos == Larva.Pos)
                    return GameState.BirdWin;
            }

            if (!birdsCanMove)
                return GameState.LarvaWin;

            // Check if the larva is in winning row
            if (Larva.Pos.Row == NUM_ROWS - 1)
                return GameState.LarvaWin;

            if (!Larva.CanMove())
                return GameState.BirdWin;

            // No winner yet
            return GameState.Running;
        }

        // Evaluate the current state using the naive heuristic we were given
        public int EvaluateHeuristic()
        {
            int larvaScore = GetScoreForPos(Larva.Pos);
            int birdScore = 0;

            foreach (var b in Birds)
            {
                birdScore += GetScoreForPos(b.Pos);
            }

            return larvaScore - birdScore;
        }

        private int GetScoreForPos(Position pos)
        {
            return (pos.Row) * 8 + pos.Col + 1;
        }

        // get all nodes on a level
        // referece - http://stackoverflow.com/questions/13349853/find-all-nodes-in-a-binary-tree-on-a-specific-level-interview-query
        private void drill(BCTree<BoardConfig> node, int cLevel, int rLevel, ref List<BCTree<BoardConfig>> result)
        {
            if (cLevel == rLevel)
                result.Add(node);
            else
            {
                foreach(BCTree<BoardConfig> b in node.children)
                {
                    drill(b, cLevel + 1, rLevel, ref result);
                }
            }
        }

        // TODO make method less repetitive
        private void generateLarvaChildren(ref BCTree<BoardConfig> parentNode, int lvl)
        {
            Console.WriteLine("ParentNode Larva = " + GetScoreForPos(parentNode.data.LarvaPos) + ", Bird 1 = " + GetScoreForPos(parentNode.data.Bird1Pos) + ", Bird 2 = " + GetScoreForPos(parentNode.data.Bird2Pos) + ", Bird 3 = " + GetScoreForPos(parentNode.data.Bird3Pos) + ", Bird 4 = " + GetScoreForPos(parentNode.data.Bird4Pos));

            Position topLeftPosition = new Position(parentNode.data.LarvaPos.Row - 1, parentNode.data.LarvaPos.Col - 1);
            // if (IsValidPosition(topLeftPosition) && IsCellEmpty(topLeftPosition))
            if (IsValidPosition(topLeftPosition) && parentNode.data.IsCellEmpty(topLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = topLeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Top left position = " + GetScoreForPos(topLeftPosition));
            }

            Position topRightPosition = new Position(parentNode.data.LarvaPos.Row - 1, parentNode.data.LarvaPos.Col + 1);
            // if (IsValidPosition(topRightPosition) && IsCellEmpty(topRightPosition))
            if (IsValidPosition(topRightPosition) && parentNode.data.IsCellEmpty(topRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = topRightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Top right position = " + GetScoreForPos(topRightPosition));
            }

            Position bottomLeftPosition = new Position(parentNode.data.LarvaPos.Row + 1, parentNode.data.LarvaPos.Col - 1);
            // if (IsValidPosition(bottomLeftPosition) && IsCellEmpty(bottomLeftPosition))
            if (IsValidPosition(bottomLeftPosition) && parentNode.data.IsCellEmpty(bottomLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = bottomLeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bottom left position = " + GetScoreForPos(bottomLeftPosition));
            }

            Position bottomRightPosition = new Position(parentNode.data.LarvaPos.Row + 1, parentNode.data.LarvaPos.Col + 1);
            // if (IsValidPosition(bottomRightPosition) && IsCellEmpty(bottomRightPosition))
            if (IsValidPosition(bottomRightPosition) && parentNode.data.IsCellEmpty(bottomRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.LarvaPos = bottomRightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bottom right position = " + GetScoreForPos(bottomRightPosition));
            }
        }

        // TODO make method less repetitive
        private void generateBirdsChildren(ref BCTree<BoardConfig> parentNode, int lvl)
        {
            Console.WriteLine("ParentNode Larva = " + GetScoreForPos(parentNode.data.LarvaPos) + ", Bird 1 = " + GetScoreForPos(parentNode.data.Bird1Pos) + ", Bird 2 = " + GetScoreForPos(parentNode.data.Bird2Pos) + ", Bird 3 = " + GetScoreForPos(parentNode.data.Bird3Pos) + ", Bird 4 = " + GetScoreForPos(parentNode.data.Bird4Pos));

            Position bird1LeftPosition = new Position(parentNode.data.Bird1Pos.Row - 1, parentNode.data.Bird1Pos.Col - 1);
            // if (IsValidPosition(bird1LeftPosition) && IsCellEmpty(bird1LeftPosition))
            if (IsValidPosition(bird1LeftPosition) && parentNode.data.IsCellEmpty(bird1LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird1Pos = bird1LeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 1 left position = " + GetScoreForPos(bird1LeftPosition));
            }

            Position bird1RightPosition = new Position(parentNode.data.Bird1Pos.Row - 1, parentNode.data.Bird1Pos.Col + 1);
            // if (IsValidPosition(bird1RightPosition) && IsCellEmpty(bird1RightPosition))
            if (IsValidPosition(bird1RightPosition) && parentNode.data.IsCellEmpty(bird1RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird1Pos = bird1RightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 1 right position = " + GetScoreForPos(bird1RightPosition));
            }

            Position bird2LeftPosition = new Position(parentNode.data.Bird2Pos.Row - 1, parentNode.data.Bird2Pos.Col - 1);
            // if (IsValidPosition(bird2LeftPosition) && IsCellEmpty(bird2LeftPosition))
            if (IsValidPosition(bird2LeftPosition) && parentNode.data.IsCellEmpty(bird2LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird2Pos = bird2LeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 2 left position = " + GetScoreForPos(bird2LeftPosition));
            }

            Position bird2RightPosition = new Position(parentNode.data.Bird2Pos.Row - 1, parentNode.data.Bird2Pos.Col + 1);
            // if (IsValidPosition(bird2RightPosition) && IsCellEmpty(bird2RightPosition))
            if (IsValidPosition(bird2RightPosition) && parentNode.data.IsCellEmpty(bird2RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird2Pos = bird2RightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 2 right position = " + GetScoreForPos(bird2RightPosition));
            }

            Position bird3LeftPosition = new Position(parentNode.data.Bird3Pos.Row - 1, parentNode.data.Bird3Pos.Col - 1);
            // if (IsValidPosition(bird3LeftPosition) && IsCellEmpty(bird3LeftPosition))
            if (IsValidPosition(bird3LeftPosition) && parentNode.data.IsCellEmpty(bird3LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird3Pos = bird3LeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 3 left position = " + GetScoreForPos(bird3LeftPosition));
            }

            Position bird3RightPosition = new Position(parentNode.data.Bird3Pos.Row - 1, parentNode.data.Bird3Pos.Col + 1);
            // if (IsValidPosition(bird3RightPosition) && IsCellEmpty(bird3RightPosition))
            if (IsValidPosition(bird3RightPosition) && parentNode.data.IsCellEmpty(bird3RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird3Pos = bird3RightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 3 right position = " + GetScoreForPos(bird3RightPosition));
            }

            Position bird4LeftPosition = new Position(parentNode.data.Bird4Pos.Row - 1, parentNode.data.Bird4Pos.Col - 1);
            // if (IsValidPosition(bird4LeftPosition) && IsCellEmpty(bird4LeftPosition))
            if (IsValidPosition(bird4LeftPosition) && parentNode.data.IsCellEmpty(bird4LeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird4Pos = bird4LeftPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 4 left position = " + GetScoreForPos(bird4LeftPosition));
            }

            Position bird4RightPosition = new Position(parentNode.data.Bird4Pos.Row - 1, parentNode.data.Bird4Pos.Col + 1);
            // if (IsValidPosition(bird4RightPosition) && IsCellEmpty(bird4RightPosition))
            if (IsValidPosition(bird4RightPosition) && parentNode.data.IsCellEmpty(bird4RightPosition))
            {
                BoardConfig tempBC = new BoardConfig(parentNode.data);
                tempBC.Level = lvl;
                tempBC.Bird4Pos = bird4RightPosition;
                parentNode.AddChild(tempBC);
                Console.WriteLine("Bird 4 right position = " + GetScoreForPos(bird4RightPosition));
            }
        }

        // TODO replace level numbers with mod-2 conditions to use mini max
        private void calculateLevelHeuristics(ref List<BCTree<BoardConfig>> lNodes)
        {
            for (int i = 0; i < lNodes.Count; ++i)
            {
                /*
                if (lNodes[i].Level == 3) // Calculate heuristics of leaves
                {
                    // TODO See if there is a better way to do this; it wasn't allowing changes to lnodes[i].data.heuristic
                    BoardConfig tempBC = new BoardConfig(lNodes[i].data);
                    int tempHeuristic = lNodes[i].data.EvaluateBCHeuristic();
                    tempBC.heuristic = tempHeuristic;
                    lNodes[i].data = tempBC;
                }
                */
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

        private Position getBestLarvaMove(List<BCTree<BoardConfig>> level1Nodes, ref BCTree<BoardConfig> rootNode)
        {
            // Calculate MAX of children and place it in root, as well as return position of MAX child
            if (rootNode.isLeaf)
            {
                BoardConfig tempBC = new BoardConfig(rootNode.data);
                int tempHeuristic = rootNode.data.EvaluateBCHeuristic();
                tempBC.heuristic = tempHeuristic;
                rootNode.data = tempBC;
                Console.WriteLine("Root is a leaf");
                return rootNode.data.LarvaPos;
            }
            else
            {
                BoardConfig tempBC = new BoardConfig(rootNode.data);
                int MAXHeuristic = rootNode.children[0].data.heuristic;
                BoardConfig MAXConfig = rootNode.children[0].data;
                for (int i = 1; i < rootNode.children.Count; ++i)
                {
                    if (rootNode.children[i].data.heuristic > MAXHeuristic)
                    {
                        MAXHeuristic = rootNode.children[i].data.heuristic;
                        MAXConfig = rootNode.children[i].data;
                    }
                }
                return MAXConfig.LarvaPos;
            }
        }

        public void AILavaMove()
        {
            // Get original positions
            Position origLarvaPosition = Larva.Pos;
            Position origBird1Position = Birds[0].Pos;
            Position origBird2Position = Birds[1].Pos;
            Position origBird3Position = Birds[2].Pos;
            Position origBird4Position = Birds[3].Pos;
            BoardConfig origBC = new BoardConfig(0, origLarvaPosition, origBird1Position, origBird2Position, origBird3Position, origBird4Position);

            // List<BoardConfig> BCList = new List<BoardConfig>();
            // TODO change name to rootTree or like
            BCTree<BoardConfig> MiniMaxTree = new BCTree<BoardConfig>(origBC);
            // BCList.Add(origBC);

            // Get level 1 kids for Larva
            generateLarvaChildren(ref MiniMaxTree, 1);

            List<BCTree<BoardConfig>> level1Nodes = new List<BCTree<BoardConfig>>();
            drill(MiniMaxTree, 0, 1, ref level1Nodes);
            Console.WriteLine("Count of nodes at Level 1 given by drill method = " + level1Nodes.Count);

            // Get level 2 kids for the Birds
            for (int i = 0; i < level1Nodes.Count; ++i)
            {
                var temp = level1Nodes[i];
                generateBirdsChildren(ref temp, 2);
            }
            // generateBirdsChildren(ref MiniMaxTree, 2);

            List<BCTree<BoardConfig>> level2Nodes = new List<BCTree<BoardConfig>>();
            drill(MiniMaxTree, 0, 2, ref level2Nodes);
            Console.WriteLine("Count of nodes at Level 2 given by drill method = " + level2Nodes.Count);

            // Get level 3 kids for the larva
            for (int i = 0; i < level2Nodes.Count; ++i)
            {
                var temp = level2Nodes[i];
                generateLarvaChildren(ref temp, 3);
            }
            
            List<BCTree<BoardConfig>> level3Nodes = new List<BCTree<BoardConfig>>();
            drill(MiniMaxTree, 0, 3, ref level3Nodes);
            Console.WriteLine("Count of nodes at Level 3 given by drill method = " + level3Nodes.Count);
            // Console.WriteLine("Larva nodes at Level 1 = " + MiniMaxTree.children.Count);
            // Console.WriteLine("Larva position at root = " + GetScoreForPos(MiniMaxTree.GetData().LarvaPos));
            // Console.WriteLine("Original Larva Position = " + GetScoreForPos(origLarvaPosition));
            // Console.WriteLine("Larva position at child 1 = " + GetScoreForPos(MiniMaxTree.GetChild(1).data.LarvaPos));
            // Console.WriteLine("Larva position at child 2 = " + GetScoreForPos(MiniMaxTree.GetChild(2).data.LarvaPos));

            // Console.WriteLine("Birds nodes at Level 1 = " + MiniMaxTree.children.Count);

            Console.WriteLine("Calculating level 3 heuristics...");

            calculateLevelHeuristics(ref level3Nodes);
            
            for (int i = 0; i < level3Nodes.Count; i++)
            {
                Console.WriteLine(i + " " + level3Nodes[i].data.heuristic);
            }

            Console.WriteLine("Calculating level 2 heuristics...");

            calculateLevelHeuristics(ref level2Nodes);

            for (int i = 0; i < level2Nodes.Count; i++)
            {
                Console.WriteLine(i + " " + level2Nodes[i].data.heuristic);
            }

            Console.WriteLine("Calculating level 1 heuristics...");

            calculateLevelHeuristics(ref level1Nodes);

            for (int i = 0; i < level1Nodes.Count; i++)
            {
                Console.WriteLine(i + " " + level1Nodes[i].data.heuristic);
            }

            Console.WriteLine("Calculating best move for Larva...");

            Position nextLarvaPosition = getBestLarvaMove(level1Nodes, ref MiniMaxTree);

            Console.WriteLine("The best next move for the Larva is to go to position " + GetScoreForPos(nextLarvaPosition));
        }

        /// <summary>
        /// Get a string representation of the board.
        /// </summary>
        /// <returns>A string representation of the board</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(_headerString);
            sb.AppendLine(_rowSeperator);

            for (int i = 0; i < NUM_ROWS; i++)
            {
                int rowNum = NUM_ROWS - i;

                sb.Append(rowNum);
                sb.Append('|');
                
                for (int j = 0; j < NUM_COLS; j++)
                {
                    GamePiece p = _data[i * NUM_COLS + j];
                    char rep = p != null ? p.CharRepresentation : ' ';
                    sb.Append(rep);
                    sb.Append('|');
                }

                sb.Append(rowNum);

                sb.AppendLine();
                sb.AppendLine(_rowSeperator);
            }
            sb.Append(_headerString);
            return sb.ToString();
        }

        // Set all cells to empty
        private void EmptyBoard()
        {
            for (int i = 0; i < NUM_CELLS; i++)
            {
                _data[i] = null;
            }
        }

        // Gets the appropriate string to be placed between rows
        private string GetRowSeperator()
        {
            var sb = new StringBuilder();

            int extraSpace = NUM_ROWS.ToString().Length;

            for (int i = 0; i < extraSpace; i++)
                sb.Append(' ');

            for (int i = 0; i < NUM_COLS * 2 + 1; i++)
                sb.Append('-');

            return sb.ToString();
        }

        // Gets the header string for the board (i.e. A B C...)
        private string GetHeaderString()
        {
            var sb = new StringBuilder();

            int extraSpace = NUM_ROWS.ToString().Length;

            for (int i = 0; i < extraSpace; i++)
                sb.Append(' ');

            int count = 0;

            for (int i = 0; i < NUM_COLS * 2 + 1; i++)
            {
                if (i % 2 == 1)
                    sb.Append((char)('A' + count++));
                else
                    sb.Append(' ');
            }

            return sb.ToString();
        }

        private bool IsPositionInMap(Position p)
        {
            return p.Row >= 0
                && p.Row < NUM_ROWS
                && p.Col >= 0
                && p.Col < NUM_COLS;
        }
    }
}
