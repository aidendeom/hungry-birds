namespace hungry_birds
{
    public class Larva : GamePiece
    {
        public Larva(Position p, Board b)
            : base(p.Row, p.Col, b)
        { }

        public override void Move(MoveDirection dir)
        {
            Position to;
            switch (dir)
            {
                case MoveDirection.UpLeft:
                    to = new Position(Pos.Row - 1, Pos.Col - 1);
                    break;
                case MoveDirection.UpRight:
                    to = new Position(Pos.Row - 1, Pos.Col + 1);
                    break;
                case MoveDirection.DownLeft:
                    to = new Position(Pos.Row + 1, Pos.Col - 1);
                    break;
                case MoveDirection.DownRight:
                    to = new Position(Pos.Row + 1, Pos.Col + 1);
                    break;
                default: // Should cover all cases
                    return;
            }

            if (!_board.IsValidPosition(to))
                return;

            Move m = new Move(Pos, to);

            _board.Move(m);

            Pos = to;
        }
    }
}
