using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int[] _stagePuzzleClearCount;
    
    private void Awake()
    {
        base.Awake();

        _stagePuzzleClearCount = new int[(int)SCENE_TYPE.Count];
        for (int i = 0; i < 6; i++)
        {
            _stagePuzzleClearCount[(int)SCENE_TYPE.Object + i] = PlayerPrefs.GetInt((SCENE_TYPE.Object + i).ToString(), 0);
        }
    }

    public void GameStart()
    {
        //게임 정보 초기화 등등
    }

    public void GameOver()
    {
        //GameOverUI 띄우고 시간 정지 등등
    }

    public void ClearPuzzle(SCENE_TYPE sceneType)
    {
        _stagePuzzleClearCount[(int)sceneType]++;
        PlayerPrefs.SetInt(sceneType.ToString(),_stagePuzzleClearCount[(int)sceneType]);
    }
    
    
}
