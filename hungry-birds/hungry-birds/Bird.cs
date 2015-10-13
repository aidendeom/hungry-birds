using System;

namespace hungry_birds
{
    public class Bird : GamePiece
    {
        // Allow for different representations to differentiate on the board;
        private char _representation = 'B';

        /// <summary>
        /// Character representation of a Bird
        /// </summary>
        public override char CharRepresentation => _representation;

        public Bird(Position pos, Board b, char representation)
            : base(pos, b)
        {
            _representation = representation;
        }

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
