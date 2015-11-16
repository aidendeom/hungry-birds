using System;
using System.Diagnostics;

namespace hungry_birds
{
    public enum GameState
    {
        Running,
        LarvaWin,
        BirdWin
    }
        

    public class Game
    {
        private Board _board = null;
        private Larva _larva = null;
        private Bird[] _birds = null;

        // Larva player
        private LarvaPlayer _player1 = null;
        // Birds player
        private BirdPlayer _player2 = null;
        private IPlayer _currentPlayer = null;

        /// <summary>
        /// Createa a new game, with a board and all peices associated with it
        /// </summary>
        public Game()
        {
            _board = new Board();
            _larva = _board.Larva;
            _birds = _board.Birds;

            _player1 = new LarvaPlayer(_larva);
            _player2 = new BirdPlayer(_birds);
            _currentPlayer = _player1;
        }

        /// <summary>
        /// Run the game
        /// </summary>
        public void PlayGame()
        {
            var state = GameState.Running;
            while (state == GameState.Running)
            {
                UpdateScreen();
                if (_currentPlayer.GetType().Name.Equals("LarvaPlayer") && Driver.AI.Equals("l"))
                {
                    // BoardConfig nextConfig = _board.AILarvaMove();
                    // _currentPlayer.DoMove(); // TODO remove
                    _currentPlayer.DoAIMove(_larva, _birds, _board); // Comment this and uncomment previous line to see heuristic but do the move yourself
                }
                else if (_currentPlayer.GetType().Name.Equals("BirdPlayer") && Driver.AI.Equals("b"))
                {
                    // BoardConfig nextConfig = _board.AIBirdsMove();
                    // _currentPlayer.DoMove(); // TODO remove
                    _currentPlayer.DoAIMove(_larva, _birds, _board); // Comment this and uncomment previous line to see heuristic but do the move yourself
                }
                else
                {
                    _currentPlayer.DoMove();
                }
                NextPlayer();
                state = _board.CheckForWinner();
            }
            UpdateScreen();
            DisplayWinner(state);
            System.Threading.Thread.Sleep(10000); // TODO NOT EXIT after winner is declared
        }

        private void UpdateScreen()
        {
            // Console.Clear();
            Console.WriteLine(_board);
            Console.WriteLine(string.Format("Current Score: {0}", _board.EvaluateHeuristic()));
        }

        /// <summary>
        /// Set the current player reference to the next player
        /// </summary>
        private void NextPlayer()
        {
            Debug.Assert(_currentPlayer != null);

            if (_currentPlayer == _player1)
                _currentPlayer = _player2;
            else
                _currentPlayer = _player1;
        }

        private void DisplayWinner(GameState winner)
        {
            string message;

            switch (winner)
            {
                case GameState.BirdWin:
                    message = "Birds win!";
                    break;
                case GameState.LarvaWin:
                    message = "Larva wins!";
                    break;
                default:
                    message = "Game's over, no winner... what?";
                    break;
            }

            Console.WriteLine(message);
        }
    }
}
