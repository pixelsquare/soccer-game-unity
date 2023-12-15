using UnityEngine;
using Quantum;

namespace SoccerGame
{
    public class GameplayHandler : MonoBehaviour
    {
        public void OnEnable()
        {
            QuantumEvent.Subscribe<EventOnGameScoreUpdated>(this, HandleGameScoreUpdated);
        }

        public void OnDisable()
        {
            QuantumEvent.UnsubscribeListener<EventOnGameScoreUpdated>(this);
        }

        private void HandleGameScoreUpdated(EventOnGameScoreUpdated callback)
        {
            var f = QuantumRunner.Default.Game.Frames.Verified;

            if (f.TryResolveDictionary<int, PlayerData>(callback.gameInfo.playerDataMap, out var playerDataMap))
            {
                foreach (var (playerRef, playerData) in playerDataMap)
                {
                    if (playerData.score < 3)
                    {
                        continue;
                    }

                    ConnectionManager.Instance.DisconnectToServer();
                    break;
                }
            }
        }
    }
}
