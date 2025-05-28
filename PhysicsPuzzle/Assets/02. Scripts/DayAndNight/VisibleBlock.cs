using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Environment;
using UnityEngine;

public class VisibleBlock : MonoBehaviour
{
    [SerializeField] private TIME_TYPE timeBlockType;
    
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeVisibleBlock(TIME_TYPE isDay)
    {
        if(timeBlockType == isDay) _meshRenderer.enabled = true;
        else _meshRenderer.enabled = false;
    }
}
