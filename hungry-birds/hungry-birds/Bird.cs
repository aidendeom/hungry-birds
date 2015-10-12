using System;

namespace hungry_birds
{
    public class Bird : GamePiece
    {
        public Bird(Position p, Board b) :
            base(p.Row, p.Col, b)
        { }

        public override void Move(MoveDirection dir)
        {
            throw new NotImplementedException();
        }
    }
}
