using System;

namespace hungry_birds
{
    public class Bird : GamePiece
    {
        public Bird(Position p, Board b) :
            base(p.Row, p.Col, b)
        { }

        public override void Move(MoveDirection dir)
        {
            // Get the new coordinate based on direction
            Position to;
            switch (dir)
            {
                case MoveDirection.UpLeft:
                    to = new Position(Pos.Row - 1, Pos.Col - 1);
                    break;
                case MoveDirection.UpRight:
                    to = new Position(Pos.Row - 1, Pos.Col + 1);
                    break;
                default: // Should cover all cases
                    throw new InvalidMoveException($"Direction {dir} is not allowed for Larva");
            }

            // Check of the new coordinate is on the board
            if (!_board.IsValidPosition(to))
                throw new InvalidMoveException($"Cannot move in direction {dir}");

            // Create a move representing this move
            Move m = new Move(Pos, to);

            // Perform the move
            _board.Move(m);

            // Update this peice's position
            Pos = to;
        }
    }
}
