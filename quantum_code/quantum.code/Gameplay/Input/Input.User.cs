namespace Quantum
{
    public partial struct Input
    {
        public Direction ToDirection()
        {
            var direction = default(Direction);

            if (MoveBack.IsDown)
            {
                direction = direction.SetFlag(Direction.Down);
            }

            if (MoveForward.IsDown)
            {
                direction = direction.SetFlag(Direction.Up);
            }

            if (MoveLeft.IsDown)
            {
                direction = direction.SetFlag(Direction.Left);
            }

            if (MoveRight.IsDown)
            {
                direction = direction.SetFlag(Direction.Right);
            }

            return direction;
        }
    }
}
