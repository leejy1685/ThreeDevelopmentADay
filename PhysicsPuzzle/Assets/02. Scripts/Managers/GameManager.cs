using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int[] _stagePuzzleClearCount;
    [SerializeField] private int stageCount;
    
    private LobbyCamera _lobbyCamera;
    private void Awake()
    {
        base.Awake();

        _stagePuzzleClearCount = new int[(int)SCENE_TYPE.Count];
        _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
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
    }

    public void StageClear()
    {
        //ClearUI 띄우고 시간 정지 등등
    }

    public void ClearPuzzle(SCENE_TYPE sceneType)
    {
        _stagePuzzleClearCount[(int)sceneType]++;
        PlayerPrefs.SetInt(sceneType.ToString(),_stagePuzzleClearCount[(int)sceneType]);
    }
    
    
}
