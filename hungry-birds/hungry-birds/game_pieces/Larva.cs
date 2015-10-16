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

        public override void Move(Move move)
        {
            if (!IsValidMove(move))
                throw new InvalidMoveException();

            _board.Move(move);

            Pos = move.To;
        }

        public override bool CanMove()
        {
            var upLeft = new Position(Pos.Row - 1, Pos.Col - 1);
            var upRight = new Position(Pos.Row - 1, Pos.Col + 1);
            var downLeft = new Position(Pos.Row + 1, Pos.Col - 1);
            var downRight = new Position(Pos.Row + 1, Pos.Col + 1);

            return _board.IsCellEmpty(upLeft)
                || _board.IsCellEmpty(upRight)
                || _board.IsCellEmpty(downLeft)
                || _board.IsCellEmpty(downRight);
        }

        protected override bool IsValidMove(Move move)
        {
            var rowDiff = Math.Abs(Pos.Row - move.To.Row);
            var colDiff = Math.Abs(Pos.Col - move.To.Col);

            var canDoMove = rowDiff == 1 && colDiff == 1;

            return canDoMove
                && _board.IsValidPosition(move.To)
                && _board.IsCellEmpty(move.To);
        }
    }
}
