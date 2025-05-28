using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Character.Player;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int[] _stagePuzzleClearCount;
    [SerializeField] private int stageCount;
    
    private LobbyCamera _lobbyCamera;

    private Transform player;
    public Door[] doors;
    public bool isLoad = false;
    private void Awake()
    {
        base.Awake();

        _stagePuzzleClearCount = new int[(int)SCENE_TYPE.Count];
        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
        
        //testCode
        PlayerPrefs.SetInt(SCENE_TYPE.ObjectAndPipe.ToString(),3);
    }

    //씬이 넘어갈 때 사용
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ClearPuzzle(SceneHandleManager.Instance.currentScene);
        }
    }
    
    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        doors = FindObjectsOfType<Door>();
        Array.Sort(doors);

        //로딩 씬에선 캐릭터가 없기 떄문에 잡히지 않음.
        player = FindAnyObjectByType<Player>()?.transform;
        if (player != null)
        {
            player.position = new Vector3(20, 0, 0);
            if (isLoad)
            {
                Debug.Log("load");
                Debug.Log(_stagePuzzleClearCount[(int)SceneHandleManager.Instance.currentScene]);
                player.position = doors[_stagePuzzleClearCount[(int)SceneHandleManager.Instance.currentScene]]
                    .PuzzleClearPosition();
            }
        }
    }
    public void GameStart()
    {
        //카메라 변경
        _lobbyCamera.DisableCamera();
        //마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
        //게임 정보 초기화
        
        //게임UI로 변경
        UIManager.Instance.ChangeState(UIState.Game);
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
    }

    public void StageClear()
    {
        //ClearUI 띄우고 시간 정지 등등
    }

    public void ClearPuzzle(SCENE_TYPE sceneType)
    {
        //문 오픈
        doors[_stagePuzzleClearCount[(int)sceneType]].DestroyDoor();
        
        //클리어 저장
        _stagePuzzleClearCount[(int)sceneType]++;
        PlayerPrefs.SetInt(sceneType.ToString(),_stagePuzzleClearCount[(int)sceneType]);
        
    }
    
    
}
