using System.Collections.Generic;
using Quantum;
using UnityEngine;

namespace SoccerGame
{
    public class ScoreboardHandler : MonoBehaviour
    {
        [SerializeField] private PlayerScorePanel _playerScorePanelRef;

        private Dictionary<int, PlayerScorePanel> _playerScorePanelMap = new Dictionary<int, PlayerScorePanel>();

        public void OnEnable()
        {
            QuantumEvent.Subscribe<EventOnGameScoreUpdated>(this, HandleGameScoreUpdated);
            QuantumEvent.Subscribe<EventOnPlayerConnected>(this, HandlePlayerConnectedEvent);
        }

        public void OnDisable()
        {
            QuantumEvent.UnsubscribeListener<EventOnGameScoreUpdated>(this);
            QuantumEvent.UnsubscribeListener<EventOnPlayerConnected>(this);
        }

        public void Start()
        {
            var f = QuantumRunner.Default.Game.Frames.Verified;
            var room = ConnectionManager.Client.CurrentRoom;

            for (var i = 1; i < room.PlayerCount + 1; i++)
            {
                var playerScorePanel = CreatePlayerScorePanel($"p{i}", 0);
                _playerScorePanelMap.TryAdd(i, playerScorePanel);
            }
        }

        private void HandleGameScoreUpdated(EventOnGameScoreUpdated callback)
        {
            var f = QuantumRunner.Default.Game.Frames.Verified;

            if (f.TryResolveDictionary<int, PlayerData>(callback.gameInfo.playerDataMap, out var playerDataMap))
            {
                foreach (var (playerRef, playerData) in playerDataMap)
                {
                    if (!_playerScorePanelMap.ContainsKey(playerRef))
                    {
                        continue;
                    }

                    _playerScorePanelMap[playerRef].SetScore(playerData.score);
                }
            }
        }

        private void HandlePlayerConnectedEvent(EventOnPlayerConnected callback)
        {
            Debug.Log("CONNECTED");
            var playerRefIdx = callback.playerRef._index;
            var playerScorePanel = CreatePlayerScorePanel($"p{playerRefIdx}");
            _playerScorePanelMap.TryAdd(playerRefIdx, playerScorePanel);
        }

        private PlayerScorePanel CreatePlayerScorePanel(string name, int score = 0)
        {
            var playerScorePanel = Instantiate(_playerScorePanelRef, transform);
            playerScorePanel.SetName(name);
            playerScorePanel.SetScore(score);
            playerScorePanel.gameObject.SetActive(true);
            return playerScorePanel;
        }
    }
}
