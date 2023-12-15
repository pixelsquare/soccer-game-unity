namespace Quantum
{
    public class PlayerSetupSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected, ISignalOnPlayerDataSet
    {
        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
            AddPlayerData(f, player._index);
            f.Events.OnPlayerConnected(player);

            Log.Debug($"OnPlayerConnected: {player}");

            if (GetPlayerCount(f) > 1)
            {
                f.Signals.OnGameStart();
                f.Signals.OnGameReset();
            }
        }

        public unsafe void OnPlayerDataSet(Frame f, PlayerRef player)
        {
            Log.Debug($"OnPlayerDataSet: {player}");

            var playerData = f.GetPlayerData(player);
            var playerPrototype = f.FindAsset<EntityPrototype>(playerData.playerPrototype.Id.Value);
            var playerEntity = f.Create(playerPrototype);

            if (f.Unsafe.TryGetPointer<PlayerTag>(playerEntity, out var playerTag))
            {
                playerTag->playerRef = player;
            }

            AddPlayerData(f, player._index);
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            Log.Debug($"OnPlayerDisconnected: {player}");

            foreach (var (entity, tag) in f.GetComponentIterator<PlayerTag>())
            {
                if (tag.playerRef != player)
                {
                    continue;
                }

                f.Destroy(entity);
            }
        }

        private unsafe void AddPlayerData(Frame f, int playerIndex, int score = 0)
        {
            if (f.TryResolveDictionary<int, PlayerData>(f.Global->playerDataMap, out var playerDataMap))
            {
                playerDataMap.TryAdd(playerIndex, new PlayerData()
                {
                    score = score,
                    blockUsed = true
                });
            }
        }

        private unsafe int GetPlayerCount(Frame f)
        {
            if (f.TryResolveDictionary<int, PlayerData>(f.Global->playerDataMap, out var playerDataMap))
            {
                return playerDataMap.Count;
            }

            return 0;
        }
    }
}
