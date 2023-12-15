using UnityEngine;
using Quantum;
using TMPro;

namespace SoccerGame
{
    public class WinCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winLabel;

        private int Wins
        {
            get => PlayerPrefs.GetInt("wins", 0);
            set => PlayerPrefs.SetInt("wins", value);
        }

        public void OnEnable()
        {
            QuantumEvent.Subscribe<EventOnGameScoreUpdated>(this, HandleGameScoreUpdated);
        }

        public void OnDisable()
        {
            QuantumEvent.UnsubscribeListener<EventOnGameScoreUpdated>(this);
        }

        public void Start()
        {
            SetWins(Wins);
        }

        private void SetWins(int wins)
        {
            _winLabel.text = $"Wins: {wins}";
        }

        private void HandleGameScoreUpdated(EventOnGameScoreUpdated callback)
        {
            var player = ConnectionManager.Client.LocalPlayer.ActorNumber;
            var f = QuantumRunner.Default.Game.Frames.Verified;

            if (f.TryResolveDictionary<int, PlayerData>(callback.gameInfo.playerDataMap, out var playerDataMap))
            {
                foreach (var (playerRef, playerData) in playerDataMap)
                {
                    if (playerData.score < 3)
                    {
                        continue;
                    }

                    if (player == playerRef)
                    {
                        Wins++;
                        SetWins(Wins);
                    }

                    break;
                }
            }
        }
    }
}
