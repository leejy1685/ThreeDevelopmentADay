using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//낮과 밤
public enum TIME_TYPE
{
    Day,
    Night,
}

//보이는 블록 또는 충돌 가능한 블록
enum VISIBLE_AND_COLLISION_TYPE
{
    Visible,
    Collision,
}

public class TimeBlock : MonoBehaviour
{
    [SerializeField] private TIME_TYPE timeBlockType;
    [SerializeField] private VISIBLE_AND_COLLISION_TYPE visibleAndCollisionType;
    
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        ChangeTimeBlock(TIME_TYPE.Day);
    }

    public void ChangeTimeBlock(TIME_TYPE isDay)
    {
        if(visibleAndCollisionType == VISIBLE_AND_COLLISION_TYPE.Collision)
        {
            if (timeBlockType == isDay)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
        else
        {
            if (timeBlockType == isDay)
            {
                _meshRenderer.enabled = true;
            }
            else
            {
                _meshRenderer.enabled = false;
            }
        }
    }
}
