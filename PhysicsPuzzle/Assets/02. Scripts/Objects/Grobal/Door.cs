using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IComparable<Door>
{
    [SerializeField] private GameObject fire;
    [SerializeField] private int puzzleNumber;
    
    public void DestroyDoor()
    {
        fire.SetActive(true);
        Destroy(gameObject,3f);
    }

    public Vector3 PuzzleClearPosition()
    {
        return transform.position + (transform.forward*2);
    }

    public int CompareTo(Door other)
    {
        return this.puzzleNumber.CompareTo(other.puzzleNumber);
    }


}
