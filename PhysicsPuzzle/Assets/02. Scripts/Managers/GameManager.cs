using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Character.Player;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private UIManager _uiManager;
    
    private int[] _stagePuzzleClearCount;
    [SerializeField] private int stageCount;
    
    private LobbyCamera _lobbyCamera;

    private Transform player;
    public Door[] doors;
    public bool isLoad = false;

    private float playTime;
    private bool isClear;
    
    private void Awake()
    {
        base.Awake();

        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();

        _stagePuzzleClearCount = new int[(int)SCENE_TYPE.Count];
        
        //testCode
        PlayerPrefs.SetInt(SCENE_TYPE.ObjectAndPipe.ToString(),3);
        
        
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
        
        //시작 카메라 연출
        _lobbyCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearPuzzle(SceneHandleManager.Instance.currentScene);
        }

        if (SceneHandleManager.Instance.currentScene != SCENE_TYPE.Lobby && !isClear)
        {
            playTime += Time.deltaTime;
            _uiManager.GameUI.UpdatePlayTime(playTime);
        }
    }
    
    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _uiManager.GameUI.ChangeSceneName();
        
        doors = FindObjectsOfType<Door>();
        Array.Sort(doors);

        //로딩 씬에선 캐릭터가 없기 떄문에 잡히지 않음.
        player = FindAnyObjectByType<Player>()?.transform;
        if (player != null)
        {
            //플레이어 배치
            player.position = new Vector3(20, 0, 0);
            if (isLoad)
            {
                Debug.Log("load");
                Debug.Log(_stagePuzzleClearCount[(int)SceneHandleManager.Instance.currentScene]);
                player.position = doors[_stagePuzzleClearCount[(int)SceneHandleManager.Instance.currentScene]]
                    .PuzzleClearPosition();
            }
        }

        //로비로 돌아오면
        if (SceneHandleManager.Instance.currentScene == SCENE_TYPE.Lobby)
        {
            _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
            GameStart();
        }
    }
    public void GameStart()
    {
        //카메라 변경
        _lobbyCamera?.DisableCamera();
        //마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
        
        //게임UI로 변경
        _uiManager.ChangeState(UIState.Game);
        _uiManager.GameUI.ChangeSceneName();
    }

    // 저장 데이터 불러오기
    public void GameLoad()
    {
        for (int i = 0; i < stageCount; i++)
        {
            _stagePuzzleClearCount[(int)SCENE_TYPE.ObjectAndPipe + i] = PlayerPrefs.GetInt((SCENE_TYPE.ObjectAndPipe + i).ToString(), 0);
        }
        
        //test
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.ObjectAndPipe);

        isLoad = true;
        
        GameStart();
    }

    public void StageClear()
    {
        Cursor.lockState = CursorLockMode.None;
        _uiManager.ChangeState(UIState.Clear);

        isClear = true;
    }

    public void ClearPuzzle(SCENE_TYPE sceneType)
    {
        //문 오픈
        doors[_stagePuzzleClearCount[(int)sceneType]].DestroyDoor();
        
        //클리어 저장
        _stagePuzzleClearCount[(int)sceneType]++;
        PlayerPrefs.SetInt(sceneType.ToString(),_stagePuzzleClearCount[(int)sceneType]);
        
        if(doors.Length == _stagePuzzleClearCount[(int)sceneType])
            StageClear();
        
    }
}
