using Quantum;
using UnityEngine;

namespace SoccerGame
{
    public class PlayerDataDispatcher : QuantumCallbacks
    {
        [SerializeField] private AssetRefEntityPrototype _playerPrototypeRef;

        public override void OnGameStart(QuantumGame game)
        {
            SendPlayerData(game);
        }

        private void SendPlayerData(QuantumGame game)
        {
            foreach (var player in game.GetLocalPlayers())
            {
                var runtimePlayer = new RuntimePlayer()
                {
                    playerPrototype = _playerPrototypeRef
                };

                game.SendPlayerData(player, runtimePlayer);
            }
        }
    }
}
