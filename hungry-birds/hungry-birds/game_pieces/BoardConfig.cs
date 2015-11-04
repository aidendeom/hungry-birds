using System;

namespace hungry_birds
{
    /// <summary>
    /// Struct to represent the positions of pieces on a board
    /// </summary>
    public struct BoardConfig
    {
        public int Level { get; set; }
        public Position LarvaPos { get; set; }
        public Position Bird1Pos { get; set; }
        public Position Bird2Pos { get; set; }
        public Position Bird3Pos { get; set; }
        public Position Bird4Pos { get; set; }
        public int heuristic { get; set; }
        
        public BoardConfig (int l, Position lp, Position b1, Position b2, Position b3, Position b4)
            : this()
        {
            Level = l;
            LarvaPos = lp;
            Bird1Pos = b1;
            Bird2Pos = b2;
            Bird3Pos = b3;
            Bird4Pos = b4;
        }

        public BoardConfig (BoardConfig bc)
            : this()
        {
            Level = bc.Level;
            LarvaPos = bc.LarvaPos;
            Bird1Pos = bc.Bird1Pos;
            Bird2Pos = bc.Bird2Pos;
            Bird3Pos = bc.Bird3Pos;
            Bird4Pos = bc.Bird4Pos;
        }
    }
}
