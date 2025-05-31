using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

[Serializable]
public class Puzzle : MonoBehaviour, IComparable<Puzzle>
{
    [SerializeField] public int puzzleNumber;
    [SerializeField] private List<GoalMachine> goalMachines;
    
    public void ClearPuzzle()
    {
        int count = 0;
        foreach (var goalMachine in goalMachines)
        {
            if(goalMachine.IsActivate)
                count++;
        }
        
        if(count >= goalMachines.Count)
        {
            GameManager.Instance.ClearPuzzle();
            this.enabled = false;
        }
    }

    public int CompareTo(Puzzle other)
    {
        return this.puzzleNumber.CompareTo(other.puzzleNumber);
    }
}
