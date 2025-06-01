using UnityEngine;

namespace _02._Scripts.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = this as T;
            }
            else { if (Instance != this) Destroy(gameObject); }
        }
    }
}
