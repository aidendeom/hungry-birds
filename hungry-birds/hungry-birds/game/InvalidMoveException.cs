using System;

namespace hungry_birds
{
    /// <summary>
    /// Exception to be thrown when a player attempts to do an invalid move
    /// </summary>
    class InvalidMoveException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidMoveException() { }

        public InvalidMoveException(string message)
            : base(message)
        { }
    }
}
