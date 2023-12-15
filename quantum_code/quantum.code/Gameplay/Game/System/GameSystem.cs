using Photon.Deterministic;

namespace Quantum
{
    public unsafe class GameSystem : SystemSignalsOnly, ISignalOnGameStart, ISignalOnGameReset
    {
        public override void OnInit(Frame f)
        {
            f.Global->playerDataMap = f.AllocateDictionary<int, PlayerData>();
        }

        /// <summary>
        /// Handles ball spawning.
        /// Called when there are two or more players.
        /// </summary>
        /// <param name="f"></param>
        public void OnGameStart(Frame f)
        {
            Log.Debug("OnGameStart");
            var ballEntity = f.Create(f.RuntimeConfig.ballPrototype);
        }

        /// <summary>
        /// Handles ball and player repositioning.
        /// </summary>
        /// <param name="f"></param>
        public void OnGameReset(Frame f)
        {
            Log.Debug("OnGameReset");
            ResetBallAtRandomPosition(f);
            ResetPlayersAtRandomPosition(f);
            ResetPlayerBlockCommand(f);
        }

        private void ResetBallAtRandomPosition(Frame f)
        {
            foreach (var (entity, ball) in f.GetComponentIterator<BallTag>())
            {
                if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var transform)
                    && f.Unsafe.TryGetPointer<PhysicsBody3D>(entity, out var body3D))
                {
                    body3D->Velocity = FPVector3.Zero;
                    body3D->AngularVelocity = FPVector3.Zero;

                    var xPos = f.RNG->Next(-8, 9); // Hardcoded can be placed on a config.
                    transform->Position = new FPVector3(xPos, 3, 0);
                }
            }
        }

        private void ResetPlayersAtRandomPosition(Frame f)
        {
            foreach (var (entity, player) in f.GetComponentIterator<PlayerTag>())
            {
                if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var transform))
                {
                    var xPos = f.RNG->Next(-8, 9);
                    var zPos = f.RNG->Next(-12, -18);
                    transform->Position = new FPVector3(xPos, 2, zPos);
                }
            }
        }

        private void ResetPlayerBlockCommand(Frame f)
        {
            if (f.TryResolveDictionary<int, PlayerData>(f.Global->playerDataMap, out var playerDataMap))
            {
                foreach (var playerData in playerDataMap)
                {
                    var pData = playerData.Value;
                    pData.blockUsed = false;
                    playerDataMap[playerData.Key] = pData;
                }
            }
        }
    }
}
