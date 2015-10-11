using System;

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

    public class Larva : GamePiece
    {
        public Larva(int r, int c, Board board)
            : base(r, c, board)
        {
        }

        public override void Move(MoveDirection dir)
        {
            Position to;
            switch (dir)
            {
                case MoveDirection.UpLeft:
                    to = new Position(Pos.Row - 1, Pos.Col - 1);
                    break;
                case MoveDirection.UpRight:
                    to = new Position(Pos.Row - 1, Pos.Col + 1);
                    break;
                case MoveDirection.DownLeft:
                    to = new Position(Pos.Row + 1, Pos.Col - 1);
                    break;
                case MoveDirection.DownRight:
                    to = new Position(Pos.Row + 1, Pos.Col + 1);
                    break;
                default: // Should cover all cases
                    return;
            }

            if (!_board.IsValidPosition(to))
                return;

            Move m = new Move(Pos, to);

            _board.Move(m);

            Pos = to;
        }
    }
}
