using System;

namespace hungry_birds
{
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