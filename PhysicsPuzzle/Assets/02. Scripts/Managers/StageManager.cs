using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;
using UnityEngine.Rendering;

public class StageManager : Singleton<StageManager>
{
    //스테이지에서 퍼즐을 관리
    [SerializeField] private Puzzle[] puzzles;
    public Puzzle[] Puzzles => puzzles;
    
    GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        foreach (Puzzle puzzle in puzzles)
        {
            puzzle.Init(this);
        }
    }
    
    public void ClearPuzzle(int count)
    {
        if (count >= puzzles[_gameManager.CurrentClearPuzzle].GetCount)
        {
            _gameManager.ClearPuzzle();
        }
    }
}
