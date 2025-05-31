using System;
using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Character.Player;
using _02._Scripts.Objects.LaserMachine;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("[Managers]")]
    private UIManager _uiManager;
    private SceneHandleManager _sceneHandle;
    //private StageManager _stageManager;
    
    [Header("[LoadData]")]
    private Transform player;   //플레이어 좌표
    public bool isLoad;         //Load
    private int lastClearPuzzle;     //마지막 퍼즐
    private const string LASTSTAGE = "LastStage";   //마지막 스테이지
    private const string LASTTIME = "LastTime";     //마지막 시간
    private const string LASTCLEARPUZZLE = "LastClearPuzzle";//마지막 퍼즐

    [Header("[ClearData]")]
    private bool isClear;
    public float playTime;
    
    [SerializeField] public LobbyCamera _lobbyCamera;   //로비 연출용 카메라
    
    private bool isGameActive;
    public bool IsGameActive { get => isGameActive; }
    
    protected override void Awake()
    {
        base.Awake();
        
        //testCode
        isLoad = false;
        PlayerPrefs.SetString(LASTSTAGE,SCENE_TYPE.ObjectAndPipe.ToString());
        PlayerPrefs.SetFloat(LASTTIME,100);
        PlayerPrefs.SetInt(LASTCLEARPUZZLE,3);
    }

    //씬이 넘어갈 때 사용
    void OnEnable()
    {
        _uiManager = UIManager.Instance;
        _sceneHandle = SceneHandleManager.Instance;
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        //시작 카메라 연출
        //_lobbyCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //ClearPuzzle();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameActive = false;
            Cursor.lockState = CursorLockMode.None;
            _uiManager.SetOptionUI();
        }

        if (_sceneHandle.currentScene != SCENE_TYPE.Lobby && !isClear)
        {
            playTime += Time.deltaTime;
            _uiManager.GameUI.UpdatePlayTime(playTime);
        }
    }
    
    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _uiManager.GameUI.ChangeSceneName();
        
        //로딩 씬에선 캐릭터가 없기 떄문에 잡히지 않음.
        player = FindAnyObjectByType<Player>()?.transform;
        if (player != null)
        {
            //플레이어 배치
            LoadPlayerPosition();
        }
        
        //스테이지 매니저 할당하기
        //_stageManager = StageManager.Instance;
    }
    
    public void GameStart()
    {
        //게임 시작 중
        isGameActive = true;
        
        //마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
        
        //게임UI로 변경
        _uiManager.ChangeState(UIState.Game);
    }
    
    public void StartGameLobby()
    {
        //데이터 초기화
        isClear = false;
        playTime = 0;
        
        //카메라 변경
        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
        _lobbyCamera?.DisableCamera();
        
        //로비에선 타이머 없음
        _uiManager.GameUI.SetTimer(false);

        GameStart();
    }

    // 저장 데이터 불러오기
    public void GameLoad()
    {
        isLoad = true;

        //마지막 정보 불러오기
        playTime = PlayerPrefs.GetFloat(LASTTIME);
        lastClearPuzzle = PlayerPrefs.GetInt(LASTCLEARPUZZLE);

        //마지막 씬 불러오기
        string sceneName = PlayerPrefs.GetString(LASTSTAGE);
        SCENE_TYPE loadScene = SCENE_TYPE.Lobby;
        for (int i = 0; i < (int)SCENE_TYPE.Count;i++)
        {
            if (sceneName == ((SCENE_TYPE)i).ToString())
            {
                loadScene = (SCENE_TYPE)i;
            }
        }

        _sceneHandle.LoadScene(loadScene);
        
        GameStart();
    }

    private void LoadPlayerPosition()
    {
        if (isLoad)
        {
            //퍼즐 해결했을 때 위치
            //스테이지 매니저로 기능 할당

            //해결 했던 퍼즐의 문 파괴
            for (int i = 0; i <= lastClearPuzzle; i++)
            {
                //스테이지 매니저로 기능 할당
            }

            isLoad = false;
        }
    }

    public void SaveClearPuzzle()
    {
        //클리어 저장
        PlayerPrefs.SetString(LASTSTAGE,_sceneHandle.currentScene.ToString());
        PlayerPrefs.SetFloat(LASTTIME,playTime);
        //PlayerPrefs.SetInt(LASTCLEARPUZZLE,CurrentClearPuzzle); 스테이지 매니저에서 퍼즐 번호 저장하기
    }
    
    public void StageClear()
    {
        //마우스 고정 해제
        Cursor.lockState = CursorLockMode.None;
        
        //타이머 멈추기
        isClear = true;

        //최고 시간초 UI에 설정
        float bestTime = PlayerPrefs.GetInt(_sceneHandle.currentScene.ToString(),0);
        if (bestTime > playTime)
        {
            bestTime = playTime;
        }
        _uiManager.SetClearUI(playTime,bestTime);
    }

    public void LoadLobbyScene()
    {
        //데이터 초기화
        isClear = false;
        playTime = 0;
        
        //카메라 변경
        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
        _lobbyCamera?.DisableCamera();
        
        //로비에선 타이머 없음
        _uiManager.GameUI.SetTimer(false);

        GameStart();
        
        _sceneHandle.LoadScene(SCENE_TYPE.Lobby);
    }
    
}
