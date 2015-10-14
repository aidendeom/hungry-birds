﻿using System;

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

        public override void Move(Move move)
        {
            if (!IsValidMove(move))
                throw new InvalidMoveException();

            _board.Move(move);
        }

        protected override bool IsValidMove(Move move)
        {
            return _board.IsValidPosition(move.To)
                && _board.IsCellEmpty(move.To);
        }
    }
}
