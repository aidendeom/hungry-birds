namespace hungry_birds
{
    /// <summary>
    /// Struct to represent a position on a board
    /// </summary>
    public struct Position
    {
        public int Row { get; }
        public int Col { get; }

        public Position(int r, int c)
        {
            Row = r;
            Col = c;
        }

        /// <summary>
        /// Create a Position object from a coordinate string
        /// </summary>
        /// <param name="coord">
        ///     The corrdinate string representing a position.
        ///     Should be of the form e1
        /// </param>
        /// <returns>A position on a board</returns>
        public static Position MakePositionFromCoord(string coord)
        {
            coord = coord.ToLower();
            char cCol = coord[0];
            char cRow = coord[1];

            int col = cCol - 'a';

            // We must do it this way because the way the board is presented to
            // the player is upside-down compared to how it is stored in data.
            // For example, row 1 in data is actually 7 on the board (assuming
            // the board has 8 rows total.
            char startingChar = (char)(Board.NUM_ROWS + '0');
            int row = startingChar - cRow;

            return new Position(row, col);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Position))
                return false;

            return this == (Position)obj;
        }

        public bool Equals(Position other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Row == p2.Row
                && p1.Col == p2.Col;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
    }
}