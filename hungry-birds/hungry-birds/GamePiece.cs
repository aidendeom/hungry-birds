namespace hungry_birds
{
    public abstract class GamePiece
    {
        // Position of the piece on the board
        public Position Pos { get; protected set; }

        // Reference to the board this piece is on
        protected Board _board = null;

        public GamePiece(int r, int c, Board board)
        {
            Pos = new Position(r, c);
            _board = board;
        }

        public abstract void Move(MoveDirection dir);
    }
}
