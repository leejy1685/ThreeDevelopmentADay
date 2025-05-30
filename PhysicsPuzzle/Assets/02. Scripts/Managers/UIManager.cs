﻿using _02._Scripts.UI;

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
    
    public GameUI GameUI => gameUI;

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
        ChangeState(UIState.Game);
    }


    public void ChangeState(UIState state)
    {
        currentState = state;
        lobbyUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        clearUI?.SetActive(currentState);
    }
}