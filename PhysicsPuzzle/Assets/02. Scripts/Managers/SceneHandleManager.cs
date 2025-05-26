using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    // 여기에 하드코딩 방지용 SceneName
    TestMain,
    LoadingScene,
    TestGoalScene,
    Object,
    Gravity,
    Pipe,
    DayAndNight,
    Time,
    Magnetic,
    Count
}
public class SceneHandleManager : Singleton<SceneHandleManager>
{
    private string nextSceneName;
    public string NextSceneName { get => nextSceneName; }

    // 로딩 씬전환 시 호출
    public void LoadScene(SCENE_TYPE sceneName)
    {
        nextSceneName = sceneName.ToString();
        SceneManager.LoadScene(SCENE_TYPE.LoadingScene.ToString());
    }
}