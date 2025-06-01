using _02._Scripts.Utils;

namespace _02._Scripts.Managers.Indestructable
{
    public class Managers : Singleton<Managers>
    {
        protected override void Awake()
        {
            if (!Instance)
            {
                Instance = this; DontDestroyOnLoad(gameObject);
            } else { if (Instance != this) Destroy(gameObject); }
        }
    }
}
