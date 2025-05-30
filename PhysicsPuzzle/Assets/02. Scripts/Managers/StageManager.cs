using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;
using UnityEngine.Rendering;

public class StageManager : Singleton<StageManager>
{
    //스테이지에서 퍼즐을 관리
    [SerializeField] private SerializedDictionary<int, List<GoalMachine>> puzzles;
    GameManager _gameManager;
    private int count;
    
    public void IncreaseActiveCount()
    {
        count++;
        
        if (puzzles[_gameManager.CurrentClearPuzzle].Count <= count)
        {
            _gameManager.ClearPuzzle();
            count = 0;
        }
        
    }

    public void DecreaseActiveCount()
    {
        count--;
    }

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;
    }
    
}
