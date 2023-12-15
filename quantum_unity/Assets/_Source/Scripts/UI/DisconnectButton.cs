using UniRx;
using UnityEngine;
using UI = UnityEngine.UI;

namespace SoccerGame
{
    [RequireComponent(typeof(UI.Button))]
    public class DisconnectButton : MonoBehaviour
    {
        [SerializeField] private UI.Button _disconnectButton = null;

        public void Awake()
        {
            if (_disconnectButton == null)
            {
                _disconnectButton = GetComponent<UI.Button>();
            }
        }

        public void OnEnable()
        {
            _disconnectButton.OnClickAsObservable()
                             .Subscribe(_ => HandleButtonClick())
                             .AddTo(this);
        }

        private void HandleButtonClick()
        {
            ConnectionManager.Instance.DisconnectToServer();
        }
    }
}
