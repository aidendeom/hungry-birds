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
            BCTree<BoardConfig> MiniMaxTree = new BCTree<BoardConfig>(origBC);
            // BCList.Add(origBC);

            // Get level 1 kids for Larva
            Position topLeftPosition = new Position(Larva.Pos.Row - 1, Larva.Pos.Col - 1);
            if (IsValidPosition(topLeftPosition) && IsCellEmpty(topLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(origBC);
                tempBC.Level = 1;
                tempBC.LarvaPos = topLeftPosition;
                MiniMaxTree.AddChild(tempBC);
                Console.WriteLine(GetScoreForPos(topLeftPosition));
            }

            Position topRightPosition = new Position(Larva.Pos.Row - 1, Larva.Pos.Col + 1);
            if (IsValidPosition(topRightPosition) && IsCellEmpty(topRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(origBC);
                tempBC.Level = 1;
                tempBC.LarvaPos = topRightPosition;
                MiniMaxTree.AddChild(tempBC);
                Console.WriteLine(GetScoreForPos(topRightPosition));
            }

            Position bottomLeftPosition = new Position(Larva.Pos.Row + 1, Larva.Pos.Col - 1);
            if (IsValidPosition(bottomLeftPosition) && IsCellEmpty(bottomLeftPosition))
            {
                BoardConfig tempBC = new BoardConfig(origBC);
                tempBC.Level = 1;
                tempBC.LarvaPos = bottomLeftPosition;
                MiniMaxTree.AddChild(tempBC);
                Console.WriteLine(GetScoreForPos(bottomLeftPosition));
            }

            Position bottomRightPosition = new Position(Larva.Pos.Row + 1, Larva.Pos.Col + 1);
            if (IsValidPosition(bottomRightPosition) && IsCellEmpty(bottomRightPosition))
            {
                BoardConfig tempBC = new BoardConfig(origBC);
                tempBC.Level = 1;
                tempBC.LarvaPos = bottomRightPosition;
                MiniMaxTree.AddChild(tempBC);
                Console.WriteLine(GetScoreForPos(bottomRightPosition));
            }
            Console.WriteLine("Count = " + MiniMaxTree.children.Count);
            Console.WriteLine("Larva position at root = " + GetScoreForPos(MiniMaxTree.GetData().LarvaPos));
            Console.WriteLine("Original Larva Position = " + GetScoreForPos(origLarvaPosition));
            Console.WriteLine("Larva position at child 1 = " + GetScoreForPos(MiniMaxTree.GetChild(1).GetData().LarvaPos));
            Console.WriteLine("Larva position at child 2 = " + GetScoreForPos(MiniMaxTree.GetChild(2).GetData().LarvaPos));

            // Use count to terminate display loop
            // Implement parent connections
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
