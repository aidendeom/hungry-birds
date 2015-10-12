using System.Text;

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

        readonly string _rowSeperator;
        readonly string _headerString;
        public readonly Position INITIA_LARVA_POS = new Position(6, 3);

        char[] _data = new char[NUM_ROWS * NUM_COLS];

        public Larva Larva { get; private set; }
        public GamePiece[] Birds { get; private set; }

        /// <summary>
        /// Create an empty board
        /// </summary>
        public Board()
        {
            EmptyBoard();
            _rowSeperator = GetRowSeperator();
            _headerString = GetHeaderString();

            Larva = new Larva(INITIA_LARVA_POS, this);
            SetCell(Larva.Pos, 'l');
        }

        public void SetCell(Position p, char c)
        {
            if (!IsPositionInMap(p))
                return;

            _data[p.Row * NUM_COLS + p.Col] = c;
        }

        public char GetCell(Position p)
        {
            if (!IsPositionInMap(p))
                return '\0';

            return _data[p.Row * NUM_COLS + p.Col];
        }

        public bool IsValidPosition(Position pos)
        {
            return IsPositionInMap(pos);
        }

        public void Move(Move m)
        {
            var cell = GetCell(m.From);
            SetCell(m.From, ' ');
            SetCell(m.To, cell);
        }

        /// <summary>
        /// Get a string representation of the board.
        /// </summary>
        /// <returns>A string representation of the board</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(_headerString);
            sb.AppendLine(_rowSeperator);

            for (int i = 0; i < NUM_ROWS; i++)
            {
                int rowNum = NUM_ROWS - i;

                sb.Append(rowNum);
                sb.Append('|');
                
                for (int j = 0; j < NUM_COLS; j++)
                {
                    sb.Append(_data[i * NUM_COLS + j]);
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
                _data[i] = ' ';
            }
        }

        // Gets the appropriate string to be placed between rows
        private string GetRowSeperator()
        {
            StringBuilder sb = new StringBuilder();

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
            StringBuilder sb = new StringBuilder();

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
