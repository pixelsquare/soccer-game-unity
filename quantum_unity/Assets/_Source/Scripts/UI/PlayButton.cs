using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UI = UnityEngine.UI;

namespace SoccerGame
{
    [RequireComponent(typeof(UI.Button))]
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private UI.Button _playButton;

        public void Awake()
        {
            if (_playButton == null)
            {
                _playButton = GetComponent<UI.Button>();
            }
        }

        public void OnEnable()
        {
            _playButton.OnClickAsObservable()
                       .Subscribe(_ => HandleButtonClick())
                       .AddTo(this);
        }

        private void HandleButtonClick()
        {
            ConnectionManager.Instance.ConnectToServer();
        }
    }
}
