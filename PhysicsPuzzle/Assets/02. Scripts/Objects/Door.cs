using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    
    public void DestroyDoor()
    {
        fire.SetActive(true);
        Destroy(gameObject,3f);
    }
    
}
