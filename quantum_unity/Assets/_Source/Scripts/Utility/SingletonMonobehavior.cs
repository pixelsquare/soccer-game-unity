using UnityEngine;

namespace SoccerGame
{
    public class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance;

        private static T _instance = null;

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
