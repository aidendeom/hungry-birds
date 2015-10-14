using System;

namespace hungry_birds
{
    public interface IPlayer
    {
        void DoMove();
    }

    /// <summary>
    /// Class for a player playing the Larva
    /// </summary>
    public class LarvaPlayer : IPlayer
    {
        private Larva _larva = null;

        public LarvaPlayer(Larva larva)
        {
            _larva = larva;
        }

        /// <summary>
        /// Prompts the player for input to create a move.
        /// </summary>
        /// <returns>A move for the player to make</returns>
        public void DoMove()
        {
            // Keep trying to get a valid move
            while (true)
            {
                try
                {
                    var move = GetMove();
                    _larva.Move(move);
                    break;
                }
                catch (InvalidMoveException) { }
            }
        }

        private Move GetMove()
        {
            // Should read a input string. e.g. e1
            string coord = Console.In.ReadLine();
            var to = Position.MakePositionFromCoord(coord);

            return new Move(_larva.Pos, to);
        }
    }

    /// <summary>
    /// Class for player playing the Birds
    /// </summary>
    public class BirdPlayer : IPlayer
    {
        public static readonly char[] SEPERATORS = { ' ' };

        private Bird[] _birds = null;

        public BirdPlayer(Bird[] birds)
        {
            _birds = birds;
        }

        public void DoMove()
        {
            // Keep trying until a valid move is produced
            while (true)
            {
                try
                {
                    var move = GetMove();
                    var bird = GetBirdToMove(move);
                    bird.Move(move);
                    break;
                }
                catch (InvalidMoveException) { }
            }
        }

        private Move GetMove()
        {
            string input = Console.In.ReadLine();
            string[] coords = input.Split(SEPERATORS);

            var from = Position.MakePositionFromCoord(coords[0]);
            var to = Position.MakePositionFromCoord(coords[1]);

            return new Move(from, to);
        }

        private Bird GetBirdToMove(Move move)
        {
            foreach (var bird in _birds)
            {
                if (bird.Pos == move.From)
                    return bird;
            }

            throw new InvalidMoveException();
        }
    }
}