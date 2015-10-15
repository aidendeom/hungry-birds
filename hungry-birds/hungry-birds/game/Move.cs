using System;

namespace hungry_birds
{
    /// <summary>
    /// Struct to represent a move in Hungry Birds
    /// </summary>
    public struct Move
    {
        public Position From { get; private set; }
        public Position To { get; private set; }

        public Move(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
}
