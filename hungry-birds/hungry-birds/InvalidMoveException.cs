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
        public InvalidMoveException()
            : base()
        { }

        /// <summary>
        /// Constructor with error message
        /// </summary>
        /// <param name="str">Error message</param>
        public InvalidMoveException(string str)
            : base(str)
        { }
    }
}
