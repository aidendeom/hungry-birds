using System;

namespace hungry_birds
{
    public class Game
    {
        private Board _board = null;
        private Larva _larva = null;
        private Bird[] _birds = null;

        public Game()
        {
            _board = new Board();
            _larva = _board.Larva;
            _birds = _board.Birds;
        }

        public void StartGame()
        {
            UpdateScreen();
            var key = Console.ReadKey(false);
            while (key.Key != ConsoleKey.Escape)
            {
                try
                {
                    var dir = GetMoveDir(key);
                    _larva.Move(dir);
                    UpdateScreen();
                }
                catch (ArgumentException) { }
                catch (InvalidMoveException e)
                {
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    int totalRows = Board.NUM_ROWS * 2 + 3;
                    Console.SetCursorPosition(0, totalRows);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, totalRows);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                key = Console.ReadKey(false);

            }
        }

        private void UpdateScreen()
        {
            Console.Clear();
            Console.WriteLine(_board);
        }

        private static MoveDirection GetMoveDir(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Q:
                    return MoveDirection.UpLeft;
                case ConsoleKey.W:
                    return MoveDirection.UpRight;
                case ConsoleKey.A:
                    return MoveDirection.DownLeft;
                case ConsoleKey.S:
                    return MoveDirection.DownRight;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
