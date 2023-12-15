using Photon.Deterministic;

namespace Quantum
{
    public class BlockSystem : SystemMainThread, ISignalOnBlockedUsed
    {
        public override void Update(Frame f)
        {
            for (var i = 0; i < f.PlayerCount; i++)
            {
                if (f.GetPlayerCommand(i) is BlockCommand blockCommand)
                {
                    blockCommand.Execute(f);
                }
            }
        }

        public unsafe void OnBlockedUsed(Frame f, PlayerRef playerRef)
        {
            var playerData = default(PlayerData);
            var playerRefIdx = playerRef._index;

            if (f.TryResolveDictionary<int, PlayerData>(f.Global->playerDataMap, out var playerDataMap)
                && playerDataMap.ContainsKey(playerRefIdx))
            {
                playerData = playerDataMap[playerRefIdx];

                if (playerData.blockUsed)
                {
                    return;
                }

                ThrowBall(f);
                playerData.blockUsed = true;
                playerDataMap[playerRefIdx] = playerData;
            }
        }

        private unsafe void ThrowBall(Frame f)
        {
            foreach (var (entity, ball) in f.GetComponentIterator<BallTag>())
            {
                if (f.Unsafe.TryGetPointer<PhysicsBody3D>(entity, out var physicsBody3D))
                {
                    Log.Debug($"{entity}");
                    physicsBody3D->AddForce(FPVector3.Back * FP._100 * 10);
                }
            }
        }
    }
}
