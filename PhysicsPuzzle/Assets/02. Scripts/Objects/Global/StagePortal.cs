using _02._Scripts.Managers.Indestructable;
using UnityEngine;

namespace _02._Scripts.Objects.Global
{
    public class StagePortal : MonoBehaviour
    {
        [SerializeField] private SCENE_TYPE SceneName;

        private void OnCollisionEnter(Collision other)
        {
            SceneHandleManager.Instance.LoadScene(SceneName);
        }
    }
}
