using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int _objectStagePuzzle;
    private int _gravityStagePuzzle;
    private int _pipeStagePuzzle;
    private int _dayAndNightStagePuzzle;
    private int _timeStagePuzzle;
    private int _magneticStagePuzzle;
    
    private void Awake()
    {
        base.Awake();
        
        _objectStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.Object.ToString(), 0);
        _gravityStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.Gravity.ToString(), 0);
        _pipeStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.Pipe.ToString(), 0);
        _dayAndNightStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.DayAndNight.ToString(), 0);
        _timeStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.Time.ToString(), 0);
        _magneticStagePuzzle = PlayerPrefs.GetInt(SCENE_TYPE.Magnetic.ToString(), 0);
    }

    public void GameStart()
    {
        //게임 정보 초기화 등등
    }

    public void GameOver()
    {
        //GameOverUI 띄우고 시간 정지 등등
    }
    
    
}
