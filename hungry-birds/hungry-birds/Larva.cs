using System;

namespace hungry_birds
{
    public class Larva : GamePiece
    {
        /// <summary>
        /// Character representation of a Larva
        /// </summary>
        public override char CharRepresentation => 'L';

        /// <summary>
        /// Create a new Larva peice on Board b at Position p
        /// </summary>
        /// <param name="p">Position at which to place the Larva</param>
        /// <param name="b">Board on which to place the Larva</param>
        public Larva(Position pos, Board b)
            : base(pos, b)
        { }

        /// <summary>
        /// Attempt to move the Larva in the provided direction.
        /// Move will fail if the calculated position is not on the board,
        /// or if it is not a valid move.
        /// </summary>
        /// <param name="dir"></param>
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
                case MoveDirection.DownLeft:
                    to = new Position(Pos.Row + 1, Pos.Col - 1);
                    break;
                case MoveDirection.DownRight:
                    to = new Position(Pos.Row + 1, Pos.Col + 1);
                    break;
                default: // Should cover all cases
                    throw new InvalidMoveException(dir);
            }

            // Check of the new coordinate is on the board
            if (!_board.IsValidPosition(to))
                throw new InvalidMoveException(dir);

            // Create a move representing this move
            Move m = new Move(Pos, to);

            // Perform the move
            _board.Move(m);

            // Update this peice's position
            Pos = to;
        }
    }
}
