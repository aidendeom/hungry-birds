using System;

namespace hungry_birds
{
    public class Bird : GamePiece
    {
        /// <summary>
        /// Character representation of a Bird
        /// </summary>
        public override char CharRepresentation { get { return 'B'; } }

        public Bird(Position pos, Board b)
            : base(pos, b)
        { }

        /// <summary>
        /// Move this piece with by the provided move. If the move is invalid,
        /// it will throw an InvalidMoveException
        /// </summary>
        /// <param name="move">The move to make</param>
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

            return _board.IsValidPosition(upLeft) && _board.IsCellEmpty(upLeft)
                || _board.IsValidPosition(upRight) && _board.IsCellEmpty(upRight);
        }

        protected override bool IsValidMove(Move move)
        {
            var rowDiff = move.To.Row - Pos.Row;
            var colDiff = Math.Abs(Pos.Col - move.To.Col);

            bool isUpperDiag = rowDiff == -1 && colDiff == 1;

            var piece = _board.GetCell(move.To);
            var emptyOrLarva = piece == null || piece is Larva;

            return isUpperDiag
                && emptyOrLarva
                && _board.IsValidPosition(move.To);
        }
    }
}
