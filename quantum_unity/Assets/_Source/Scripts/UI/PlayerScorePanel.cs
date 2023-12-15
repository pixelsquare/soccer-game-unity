using TMPro;
using UnityEngine;

namespace SoccerGame
{
    public class PlayerScorePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _scoreLabel;

        public void SetName(string name)
        {
            _nameLabel.text = name;
        }

        public void SetScore(int score)
        {
            SetScore($"{score}");
        }

        public void SetScore(string score)
        {
            _scoreLabel.text = score;
        }
    }
}
