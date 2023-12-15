using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoccerGame
{
    public class AppInit : MonoBehaviour
    {
        public async UniTask Start()
        {
            await SceneLoader.Instance.LoadSceneAsync(SceneNames.Persistent);
            await SceneLoader.Instance.LoadSceneAsync(SceneNames.MainMenu);
            SceneLoader.Instance.SetSceneActive(SceneNames.MainMenu);
            await SceneLoader.Instance.UnloadSceneAsync(SceneNames.Boot);
        }
    }
}
