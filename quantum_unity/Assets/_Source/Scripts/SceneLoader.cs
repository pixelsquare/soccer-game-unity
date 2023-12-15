using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace SoccerGame
{
    public class SceneLoader
    {
        public static SceneLoader Instance => _instance ??= new SceneLoader();
        private static SceneLoader _instance = null;

        public SceneLoader()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.sceneUnloaded += HandleSceneUnloaded;
        }

        ~SceneLoader()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneUnloaded -= HandleSceneUnloaded;
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            await SceneManager.UnloadSceneAsync(sceneName);
        }

        public void SetSceneActive(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }

        private async void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene Loaded: {scene.name}");

            switch (scene.name)
            {
                case SceneNames.Game:
                {
                    await UnloadSceneAsync(SceneNames.MainMenu);
                    break;
                }
            }
        }

        private async void HandleSceneUnloaded(Scene scene)
        {
            Debug.Log($"Scene Unloaded: {scene.name}");

            switch (scene.name)
            {
                case SceneNames.Game:
                {
                    await LoadSceneAsync(SceneNames.MainMenu);
                    SetSceneActive(SceneNames.MainMenu);
                    break;
                }
            }
        }
    }
}
