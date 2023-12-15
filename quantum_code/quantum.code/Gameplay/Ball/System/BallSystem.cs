namespace Quantum
{
    public class BallSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D, ISignalOnCollisionEnter3D
    {
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            Log.Debug($"OnTriggerEnter3D: {info.Entity} | {info.Other}");
            HandlePlayerScore(f, info.Entity, info.Other);
            HandleGoalCollision(f, info.Entity, info.Other);
        }

        public void OnCollisionEnter3D(Frame f, CollisionInfo3D info)
        {
            Log.Debug($"OnCollisionEnter3D: {info.Entity} | {info.Other}");
            HandleBallCollision(f, info.Entity, info.Other);
        }

        private void HandleGoalCollision(Frame f, EntityRef ballEntity, EntityRef goalEntity)
        {
            if (!f.Has<BallTag>(ballEntity))
            {
                return;
            }

            f.Signals.OnGameReset();
        }

        private unsafe void HandlePlayerScore(Frame f, EntityRef ballEntity, EntityRef goalEntity)
        {
            if (!f.Has<BallTag>(ballEntity))
            {
                return;
            }

            if (f.TryGet<BallOwner>(ballEntity, out var ballOwner))
            {
                Log.Debug($"Ball Owner: {ballOwner.playerRef}");

                if (f.TryResolveDictionary<int, PlayerData>(f.Global->playerDataMap, out var playerDataMap))
                {
                    var playerRefIdx = ballOwner.playerRef._index;
                    var playerData = playerDataMap[playerRefIdx];
                    playerData.score++;
                    playerDataMap[playerRefIdx] = playerData;
                }

                f.Events.OnGameScoreUpdated(new GameInfo()
                {
                    playerDataMap = f.Global->playerDataMap
                });
            }
        }

        private unsafe void HandleBallCollision(Frame f, EntityRef ballEntity, EntityRef playerEntity)
        {
            if (!f.Has<BallTag>(ballEntity) && !f.Has<PlayerTag>(playerEntity))
            {
                return;
            }

            if (f.Unsafe.TryGetPointer<BallOwner>(ballEntity, out var ballOwner)
                && f.TryGet<PlayerTag>(playerEntity, out var playerTag))
            {
                ballOwner->playerRef = playerTag.playerRef;
            }
        }
    }
}
