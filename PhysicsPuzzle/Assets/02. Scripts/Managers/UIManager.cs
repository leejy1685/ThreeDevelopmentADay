using _02._Scripts.UI;
using UnityEngine;

public enum UIState
{
    Lobby,
    Game,
    Clear,
    Obtion,
    ObjectAndPipe,
    LoadingScene
}

public class UIManager : Singleton<UIManager>
{
    public UIState currentState = UIState.Lobby;
    LobbyUI lobbyUI = null;
    ObtionUI obtionUI = null;
    GameUI gameUI = null;
    ClearUI clearUI = null;
    
    public GameUI GameUI => gameUI;

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

    public void SetClearUI(float time,float bestTime)
    {
        clearUI.SetClearTime(time,bestTime);
        ChangeState(UIState.Clear);
    }
}

