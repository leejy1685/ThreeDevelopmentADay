using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

[Serializable]
public class Puzzle : MonoBehaviour
{
    [SerializeField] private List<GoalMachine> goalMachines;
    [SerializeField] private int count;
    [SerializeField] private  StageManager stageManager;

    public int GetCount => goalMachines.Count;

    public void Init(StageManager stageManager)
    {
        this.stageManager = stageManager;
        count = 0;
    }

    public void IncreaseCount()
    {
        count++;
        stageManager.ClearPuzzle(count);
    }

    public void decreaseCount()
    {
        count--;
        count = Mathf.Clamp(count, 0, goalMachines.Count);
    }
    
    
    
}
