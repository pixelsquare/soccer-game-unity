namespace Quantum
{
    public unsafe class InputSystem : SystemMainThreadFilter<InputSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerTag* PlayerTag;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerTag->playerRef);

            if (f.Unsafe.TryGetPointer<Movement>(filter.Entity, out var movement))
            {
                movement->direction = input->ToDirection();
            }
        }
    }
}
