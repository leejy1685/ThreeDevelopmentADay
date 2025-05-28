using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    // 여기에 하드코딩 방지용 SceneName
    LoadingScene,
    Lobby,
    ObjectAndPipe,
    DayAndNightGravity,
    Count,
}
public class SceneHandleManager : Singleton<SceneHandleManager>
{
    
    public SCENE_TYPE currentScene;
    private string nextSceneName;
    public string NextSceneName { get => nextSceneName; }
    
    //씬이 넘어갈 때 사용
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < (int)SCENE_TYPE.Count; i++)
        {
            if (scene.name == ((SCENE_TYPE)i).ToString())
            {
                currentScene = ((SCENE_TYPE)i);
            }
        }
        
    }

    // 로딩 씬전환 시 호출
    public void LoadScene(SCENE_TYPE sceneName)
    {
        nextSceneName = sceneName.ToString();
        SceneManager.LoadScene(SCENE_TYPE.LoadingScene.ToString());
    }
}