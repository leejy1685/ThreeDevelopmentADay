using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePotal : MonoBehaviour
{
    [SerializeField] private SCENE_TYPE SceneName;

    private void OnCollisionEnter(Collision other)
    {
        SceneHandleManager.Instance.LoadScene(SceneName);
    }
}
