using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IComparable<Door>
{
    [SerializeField] private GameObject fire;
    [SerializeField] private int puzzleNumber;

    private void OnCollisionEnter(Collision other)
    {
        //test code
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.ObjectAndPipe);
    }
    
    public void DestroyDoor()
    {
        fire.SetActive(true);
        Destroy(gameObject,3f);
    }

    public int CompareTo(Door other)
    {
        return this.puzzleNumber.CompareTo(other.puzzleNumber);
    }


}
