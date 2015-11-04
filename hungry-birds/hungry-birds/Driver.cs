using System;
using hungry_birds;

class Driver
{
    public static string AI = null;

    public static void Main(string[] args)
    {
        Console.WriteLine("AI plays as: ");
        
        // Get whether computer is Larva ('l') or Birds ('b') or no one ('n')
        AI = Console.In.ReadLine();

        var game = new Game();

        game.PlayGame();
    }
}