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
        public Position[] BirdsPos { get; set; }
        public int heuristic { get; set; }

        public BoardConfig(int l, Position lp, Position[] BsP)
            : this()
        {
            Level = l;
            LarvaPos = lp;
            BirdsPos = new Position[BsP.Length];
            for (int i = 0; i < BirdsPos.Length; ++i)
            {
                BirdsPos[i] = BsP[i];
            }
        }

        public BoardConfig (BoardConfig bc)
            : this()
        {
            Level = bc.Level;
            LarvaPos = bc.LarvaPos;
            BirdsPos = new Position[bc.BirdsPos.Length];
            for (int i = 0; i < bc.BirdsPos.Length; ++i)
            {
                BirdsPos[i] = bc.BirdsPos[i];
            }
        }

        private int GetScoreForPos(Position pos)
        {
            return (pos.Row) * 8 + pos.Col + 1;
        }

        public bool IsCellEmpty(Position pos)
        {
            if (pos == LarvaPos)
                return false;

            foreach (var p in BirdsPos)
            {
                if (pos == p)
                    return false;
            }

            return true;
        }

        public int EvaluateBCHeuristic()
        {
            int larvaPosScore = GetScoreForPos(LarvaPos);
            int birdsScore = 0;

            for (int i = 0; i < BirdsPos.Length; ++i)
            {
                birdsScore += GetScoreForPos(BirdsPos[i]);
            }

            return larvaPosScore - birdsScore;
        }
    }
}
