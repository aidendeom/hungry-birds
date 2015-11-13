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

        // TODO REMOVE
        public void DoAIMove(BoardConfig bc)
        {
            int birdIndex = 4;
            var to = new Position();
            // TODO use array of BirdPos in BoardConfig to streamline process
            if (!(bc.Bird1Pos.Equals(_birds[0].Pos)))
            {
                birdIndex = 0;
                to = bc.Bird1Pos;
            }
            else if (!(bc.Bird2Pos.Equals(_birds[1].Pos)))
            {
                birdIndex = 1;
                to = bc.Bird2Pos;
            }
            else if (!(bc.Bird3Pos.Equals(_birds[2].Pos)))
            {
                birdIndex = 2;
                to = bc.Bird3Pos;
            }
            else if (!(bc.Bird4Pos.Equals(_birds[3].Pos)))
            {
                birdIndex = 3;
                to = bc.Bird4Pos;
            }
  
            try
            {
                _birds[birdIndex].Move(new Move(_birds[birdIndex].Pos, to));
            }
            catch (InvalidMoveException) { }
        }

        private Move GetMove()
        {
            try
            {
                string input = Console.In.ReadLine();
                string[] coords = input.Split(SEPERATORS);

                var from = Position.MakePositionFromCoord(coords[0]);
                var to = Position.MakePositionFromCoord(coords[1]);

                return new Move(from, to);
            }
            catch (Exception) { throw new InvalidMoveException(); }
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