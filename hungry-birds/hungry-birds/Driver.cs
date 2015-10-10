using System;
using hungry_birds;

class Driver
{
    class Larva
    {
        public int row = 6;
        public int col = 3;

        public int lrow = 6;
        public int lcol = 3;
    }

    static Board b = new Board();
    static Larva l = new Larva();
    static char full = 'l';
    static char empty = ' ';

    public static void Main(string[] args)
    {
        UpdateBoard();
        UpdateScreen();
        var key = Console.ReadKey(false);
        while (key.Key != ConsoleKey.Escape)
        {
            Move(key);
            UpdateBoard();
            UpdateScreen();
            key = Console.ReadKey(false);
        }
    }

    static void UpdateBoard()
    {
        b.SetCell(l.lrow, l.lcol, empty);
        b.SetCell(l.row, l.col, full);
    }

    static void UpdateScreen()
    {
        Console.Clear();
        Console.WriteLine(b.ToString());
    }

    static void Move(ConsoleKeyInfo key)
    {
        int r = l.row;
        int c = l.col;

        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                r--;
                break;
            case ConsoleKey.DownArrow:
                r++;
                break;
            case ConsoleKey.LeftArrow:
                c--;
                break;
            case ConsoleKey.RightArrow:
                c++;
                break;
            default:
                return;
        }

        if (b.IsValidPosition(r, c))
        {
            l.lrow = l.row;
            l.lcol = l.col;

            l.row = r;
            l.col = c;
            UpdateBoard();
        }
    }
}