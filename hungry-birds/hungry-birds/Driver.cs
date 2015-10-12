using System;
using hungry_birds;

class Driver
{
    static Board b = new Board();
    static Larva l;

    public static void Main(string[] args)
    {
        l = b.Larva;
        UpdateScreen();
        var key = Console.ReadKey(false);
        while (key.Key != ConsoleKey.Escape)
        {
            try
            {
                var dir = GetMoveDir(key);
                l.Move(dir);
                UpdateScreen();
            }
            catch (ArgumentException) { }

            key = Console.ReadKey(false);

        }
    }

    private static void UpdateScreen()
    {
        Console.Clear();
        Console.WriteLine(b);
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