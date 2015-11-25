namespace hungry_birds
{
    public interface IPlayer
    {
        void DoMove();
        void DoAIMove(Larva Larva, Bird[] Birds, Board Board);
    }
}
