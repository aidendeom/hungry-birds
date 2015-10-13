namespace hungry_birds
{
    /// <summary>
    /// Abstract representation of a GamePiece that players will interact with
    /// </summary>
    public abstract class GamePiece
    {
        // Position of the piece on the board
        public Position Pos { get; protected set; }

        // Reference to the board this piece is on
        protected Board _board = null;

        /// <summary>
        /// Return a character representation for this GamePiece
        /// </summary>
        public abstract char CharRepresentation { get; }

        /// <summary>
        /// Create a new GamePiece on provided Board at specified Position
        /// </summary>
        /// <param name="pos">Position at which to place the GamePiece</param>
        /// <param name="board">Board on which the GamePeice will exist</param>
        public GamePiece(Position pos, Board board)
        {
            Pos = pos;
            _board = board;
        }

        /// <summary>
        /// Method to move the GamePiece. Logic should be implemented in child classes
        /// </summary>
        /// <param name="dir">Direction in which to move</param>
        public abstract void Move(MoveDirection dir);
    }
}
