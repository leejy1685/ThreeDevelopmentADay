using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int[] _stagePuzzleClearCount;
    [SerializeField] private int stageCount;
    private void Awake()
    {
        base.Awake();

        _stagePuzzleClearCount = new int[(int)SCENE_TYPE.Count];
    }

    public void GameStart()
    {
        //게임 정보 초기화 등등
        //게임UI로 변경
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
