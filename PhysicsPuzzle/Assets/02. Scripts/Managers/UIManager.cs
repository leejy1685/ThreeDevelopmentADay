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
    // [Header("[현재 스테이지]")]
    // private TextMeshProUGUI _stageTitle;
    // [SerializeField] private TextMeshProUGUI _stageName;
    //
    // [Header("[남은 시간]")]
    // private TextMeshProUGUI _timeTitle;
    // [SerializeField] private TextMeshProUGUI _timeName;
    //
    // [Header("[라운드별 종료시간 설정]")]
    // private float remainingTime = 300f;
    //
    // // 상속받은 Singleton.Awake() 호출하고 DontDestroy 설정
    // private void Awake()
    // {
    //     base.Awake();
    //     DontDestroyOnLoad(gameObject);
    // }
    //
    // // 현재 스테이지 이름 설정
    // private void Start()
    // {
    //     _stageName.text = SceneManager.GetActiveScene().name;
    // }
    //
    // // 남은시간 업데이트
    // private void Update()
    // {
    //     if (remainingTime > 0f)
    //     {
    //         remainingTime -= Time.deltaTime;
    //         UpdateTimerText();
    //     }
    //     else
    //     {
    //         remainingTime = 0f;
    //         Debug.Log("Game Over!");
    //     }
    // }
    //
    // // 시간 n분:n초 형식으로 포맷팅
    // private void UpdateTimerText()
    // {
    //     int minutes = Mathf.FloorToInt(remainingTime / 60f);
    //     int seconds = Mathf.FloorToInt(remainingTime % 60f);
    //     _timeName.text = $"{minutes:00}:{seconds:00}"; 
    // }

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