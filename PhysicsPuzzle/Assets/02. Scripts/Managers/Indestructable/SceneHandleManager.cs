using _02._Scripts.Utils;
using UnityEngine.SceneManagement;

namespace _02._Scripts.Managers.Indestructable
{
    public enum SCENE_TYPE
    {
        // 여기에 하드코딩 방지용 SceneName
        LoadingScene,
        Lobby,
        ObjectAndPipe,
        DayAndNightGravity,
    }
    public class SceneHandleManager : Singleton<SceneHandleManager>
    {
        
        public SCENE_TYPE currentScene;
        private string nextSceneName;
        public string NextSceneName { get => nextSceneName; }
    
        protected override void Awake()
        {
            base.Awake();
            
            currentScene = SCENE_TYPE.Lobby;
        }
    
        // 로딩 씬전환 시 호출
        public void LoadScene(SCENE_TYPE sceneName)
        {
            currentScene = sceneName;
            nextSceneName = sceneName.ToString();
            SceneManager.LoadScene(SCENE_TYPE.LoadingScene.ToString());
        }
    }
}
