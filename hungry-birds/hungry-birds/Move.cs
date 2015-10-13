namespace hungry_birds
{
    /// <summary>
    /// Struct to represent a position on a board
    /// </summary>
    public struct Position
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Position(int r, int c)
        {
            Row = r;
            Col = c;
        }
    }
    /// <summary>
    /// Enum to define the direction in which a piece can move
    /// </summary>
    public enum MoveDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    /// <summary>
    /// Struct to represent a move in Hungry Birds
    /// </summary>
    public struct Move
    {
        public Position From { get; private set; }
        public Position To { get; private set; }
        public MoveDirection Direction { get; private set; }

        public Move(Position from, Position to)
        {
            From = from;
            To = to;
            Direction = GetDirection(from, to);
        }

        public static MoveDirection GetDirection(Position from, Position to)
        {
            if (to.Row - from.Row > 0)
            {
                if (to.Col - from.Col > 0)
                    return MoveDirection.DownRight;
                else
                    return MoveDirection.DownLeft;
            }
            else
            {
                if (to.Col - from.Col > 0)
                    return MoveDirection.UpRight;
                else
                    return MoveDirection.UpLeft;
            }
        }
    }
}
