using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Character.Player;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("[Managers]")]
    private UIManager _uiManager;
    private SceneHandleManager _sceneHandle;
    
    [Header("[LoadData]")]
    public bool isLoad;         //Load
    private int lastClearPuzzle;     //스테이지 수
    private const string LASTSTAGE = "LastStage";   //마지막 스테이지
    private const string LASTTIME = "LastTime";     //마지막 시간
    private const string LASTCLEARPUZZLE = "LastClearPuzzle";//마지막 퍼즐

    [Header("[ClearData]")]
    private bool isClear;
    public float playTime;
    private int currentClearPuzzle;   //스테이지 별 클리어 퍼즐 수
    
    private LobbyCamera _lobbyCamera;   //로비 연출용 카메라
    
    public Door[] doors;    //퍼즐 클리어 시 열리는 문
    private Transform player;   //플레이어 좌표
    
    
    private void Awake()
    {
        base.Awake();

        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
        
        //testCode
        isLoad = false;
        PlayerPrefs.SetString(LASTSTAGE,SCENE_TYPE.ObjectAndPipe.ToString());
        PlayerPrefs.SetFloat(LASTTIME,100);
        PlayerPrefs.SetInt(LASTCLEARPUZZLE,3);
    }

    //씬이 넘어갈 때 사용
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _uiManager = UIManager.Instance;
        _sceneHandle = SceneHandleManager.Instance;
        
        //시작 카메라 연출
        _lobbyCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearPuzzle();
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
        
        //문 찾기
        doors = FindObjectsOfType<Door>();
        Array.Sort(doors);

        //로딩 씬에선 캐릭터가 없기 떄문에 잡히지 않음.
        player = FindAnyObjectByType<Player>()?.transform;
        if (player != null)
        {
            //플레이어 배치
            player.position = new Vector3(20, 0, 0); //일단은 하드코딩
            LoadPlayerPosition();
        }

        //로비로 돌아오면
        if (_sceneHandle.currentScene == SCENE_TYPE.Lobby)
        {
            _uiManager.ChangeState(UIState.Lobby);
            StartGameLobby();
        }
        else//로비가 아니면 타이머 UI 생성
        {
            _uiManager.GameUI.SetTimer(true);
            //StartCoroutine(itemManager.Instnace.)
        }
    }
    
    public void GameStart()
    {
        //마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
        
        //게임UI로 변경
        _uiManager.ChangeState(UIState.Game);
    }
    
    public void StartGameLobby()
    {
        //데이터 초기화
        playTime = 0;
        isClear = false;
        currentClearPuzzle = 0;
        
        //카메라 변경
        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
        _lobbyCamera.DisableCamera();
        
        GameStart();
        
        //로비에선 타이머 없음
        _uiManager.GameUI.SetTimer(false);
        
    }

    // 저장 데이터 불러오기
    public void GameLoad()
    {
        isLoad = true;

        //마지막 정보 불러오기
        playTime = PlayerPrefs.GetFloat(LASTTIME);
        lastClearPuzzle = PlayerPrefs.GetInt(LASTCLEARPUZZLE);
        
        GameStart();

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
    }

    private void LoadPlayerPosition()
    {
        if (isLoad)
        {
            //퍼즐 해결했을 때 위치
            player.position = doors[currentClearPuzzle].PuzzleClearPosition();

            //해결 했던 퍼즐의 문 파괴
            for (int i = 0; i <= lastClearPuzzle; i++)
            {
                ClearPuzzle();
            }

            isLoad = false;
        }
    }

    public void ClearPuzzle()
    {
        //문 오픈
        doors[currentClearPuzzle].DestroyDoor();
        
        //클리어 저장
        PlayerPrefs.SetString(LASTSTAGE,_sceneHandle.currentScene.ToString());
        PlayerPrefs.SetFloat(LASTTIME,playTime);
        PlayerPrefs.SetInt(LASTCLEARPUZZLE,currentClearPuzzle);
        
        //퍼즐 해결 카운트
        currentClearPuzzle++;

        if (doors.Length == currentClearPuzzle) 
            StageClear();
        
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
}
