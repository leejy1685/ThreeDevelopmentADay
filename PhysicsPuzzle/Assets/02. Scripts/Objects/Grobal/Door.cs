using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IComparable<Door>
{
    [SerializeField] private GameObject door;
    [SerializeField] private int puzzleNumber;
    

    public void OpenDoor()
    {
        StartCoroutine(OpenDoor_Coroutine());
    }

    private IEnumerator OpenDoor_Coroutine()
    {
        while (true)
        {
            Vector3 openDoorPosition = transform.position + new Vector3(0, 0, 1.3f);
            Vector3 doorPosition = Vector3.Lerp(door.transform.position, openDoorPosition, Time.deltaTime);
            door.transform.position = doorPosition;
            yield return null;
            if (openDoorPosition == doorPosition)
            {
                break;
            }
        }
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
