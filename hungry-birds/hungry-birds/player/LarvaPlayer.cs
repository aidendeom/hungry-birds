using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hungry_birds
{
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

        // TODO REMOVE!
        public void DoAIMove(BoardConfig bc)
        {
            try
            {
                _larva.Move(new Move(_larva.Pos, bc.LarvaPos));
            }
            catch (InvalidMoveException) { }
        }

        private Move GetMove()
        {
            // Should read a input string. e.g. e1
            string coord = Console.In.ReadLine();
            var to = Position.MakePositionFromCoord(coord);

            return new Move(_larva.Pos, to);
        }
    }
}
