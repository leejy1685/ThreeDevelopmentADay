using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
    Lobby,
    Game,
    Clear,
    Obtion
}


public class UIManager : Singleton<UIManager>
{
    
    UIState currentState = UIState.Lobby;
    LobbyUI lobbyUI = null;
    ObtionUI obtionUI = null;
    GameUI gameUI = null;
    ClearUI clearUI = null;
    
    public GameUI GameUI => gameUI;
    public ClearUI ClearUI => clearUI;
    
    private float currentClearTime;
    public float CurrentClearTime => currentClearTime;
    
    private void Awake()
    {
        base.Awake();
        
        // 자식 오브젝트에서 각각의 UI를 찾아 초기화
        lobbyUI = GetComponentInChildren<LobbyUI>(true);
        lobbyUI?.Init(this);
        obtionUI = GetComponentInChildren<ObtionUI>(true);
        obtionUI?.Init(this);
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI?.Init(this);
        clearUI = GetComponentInChildren<ClearUI>(true);
        clearUI?.Init(this);
        
        // 초기 상태를 로비 화면으로 설정
        ChangeState(UIState.Lobby);
    }



    public void ChangeState(UIState state)
    {
        currentState = state;
        lobbyUI?.SetActive(currentState);
        obtionUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        clearUI?.SetActive(currentState);
    }
    
    public void OnClickStart()
    {
        ChangeState(UIState.Game);
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.Lobby); // 게임매니저에서 씬 전환을 통합할지 확인
    }
    
    public void OnClickExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public float LoadBestScore(SCENE_TYPE sceneType)
    {
        int clearCount = PlayerPrefs.GetInt(sceneType.ToString(), 0);
        
        if (clearCount == 0)
            return 0f;

        return PlayerPrefs.GetFloat($"{sceneType}_BestClearTime", 0f);
    }

    public void SetClearUI(float time,float bestTime)
    {
        clearUI.SetClearTime(time,bestTime);
        ChangeState(UIState.Clear);
    }
}
