using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
    Lobby,
    Game,
    Clear,
}

public class UIManager : Singleton<UIManager>
{
    UIState currentState = UIState.Lobby;
    LobbyUI lobbyUI = null;
    GameUI gameUI = null;
    ClearUI clearUI = null;
    
    [SerializeField] private Canvas lobbyCanvas;
    [SerializeField] private Canvas optioinCanvas;

    private void Awake()
    {
        base.Awake();
        
        // 자식 오브젝트에서 각각의 UI를 찾아 초기화
        lobbyUI = GetComponentInChildren<LobbyUI>(true);
        lobbyUI?.Init(this);
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
        gameUI?.SetActive(currentState);
        clearUI?.SetActive(currentState);
    }
    
    public void OnClickStart()
    {
        ChangeState(UIState.Game);
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.TestMain); // 게임매니저에서 씬 전환을 통합할지 확인
    }

    public void OnClickLoad()
    {
        ChangeState(UIState.Game);
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.TestMain); // 게임매니저에서 씬 전환을 통합할지 확인
    }
    
    public void OnClickOption()
    {
        lobbyCanvas.gameObject.SetActive(false);
        optioinCanvas.gameObject.SetActive(true);
    }
    
    public void OnClickExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
