namespace hungry_birds
{
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

    public abstract class GamePiece
    {
        // Position of the piece on the board
        public Position Position { get; private set; }

        // Reference to the board this piece is on
        private Board _board = null;

        public GamePiece(int r, int c, Board board)
        {
            Position = new Position(r, c);
            _board = board;
        }
    }
}
