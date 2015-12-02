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
            /*
             The board's score looks like the following

             1	2	3	4	4	3	2	1
             2	4	6	8	8	6	4	2
             3	6	9	12	12	9	6	3
             4	8	12	16	16	12	8	4
             5	10	15	20	20	15	10	5
             6	12	18	24	24	18	12	6
             7	14	21	28	28	21	14	7
             8	16	24	32	32	24	16	8
             */


            int rowScore = pos.Row + 1;
            
            int dist1 = Math.Abs(pos.Col - 3);
            int dist2 = Math.Abs(pos.Col - 4);
            int distFromCenter = Math.Min(dist1, dist2);
            int colScore = 4 - distFromCenter;

            return rowScore * colScore;
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
            // Multiply by this constant to allow for more wiggle room when dividing by float (and truncating to int).
            // Really just a hack, so I don't have to change the type of the heuristic value
            // (should be a floating type)
            int larvaPosScore = GetScoreForPos(LarvaPos);
            int birdsScore = 0;

            for (int i = 0; i < BirdsPos.Length; ++i)
            {
                // Divide by distance. Birds want to converge towards larva
                double birdScore = GetScoreForPos(BirdsPos[i]) / Position.Distance(BirdsPos[i], LarvaPos);
                // Birds above the larva have big scores, to discourage that
                if (LarvaPos.Row - BirdsPos[i].Row >= 2)
                    birdScore = 0;
                birdsScore += (int)birdsScore;
            }

            return larvaPosScore - birdsScore;
        }
    }
}
