using System;

namespace hungry_birds
{
    /// <summary>
    /// Exception to be thrown when a player attempts to do an invalid move
    /// </summary>
    class InvalidMoveException : Exception
    {
        public MoveDirection Direction { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidMoveException(MoveDirection direction)
            : base($"You can't move in direction {direction}")
        {
            Direction = direction;
        }
    }
}
