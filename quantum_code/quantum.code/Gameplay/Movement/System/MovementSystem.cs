using Photon.Deterministic;

namespace Quantum
{
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public Movement* Movement;
            public PhysicsBody3D* PhysicsB3D;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var movement = filter.Movement;
            var transform = filter.Transform;
            var body3D = filter.PhysicsB3D;

            var config = f.FindAsset<MovementConfig>(movement->config.Id);

            body3D->AddForce(GetMoveVector(movement->direction) * config.Force);
        }

        private FPVector3 GetMoveVector(Direction direction)
        {
            return direction switch
            {
                Direction.Up => FPVector3.Forward,
                Direction.Down => FPVector3.Back,
                Direction.Left => FPVector3.Left,
                Direction.Right => FPVector3.Right,
                _ => FPVector3.Zero
            };
        }
    }
}
