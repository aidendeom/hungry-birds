using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        readonly string _rowSeperator;
        readonly string _headerString;

        char[] _data = new char[NUM_ROWS * NUM_COLS];

        /// <summary>
        /// Create an empty board
        /// </summary>
        public Board()
        {
            EmptyBoard();
            _rowSeperator = GetRowSeperator();
            _headerString = GetHeaderString();
        }

        public void SetCell(int row, int col, char c)
        {
            if (!IsValidPosition(row, col))
                return;

            _data[row * NUM_COLS + col] = c;
        }

        public char GetCell(int row, int col)
        {
            if (!IsValidPosition(row, col))
                return '\0';

            return _data[row * NUM_COLS + col];
        }

        public bool IsValidPosition(int row, int col)
        {
            return row >= 0
                && row < NUM_ROWS
                && col >= 0
                && col < NUM_COLS;
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
    }
}
