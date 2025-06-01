using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.Objects.Blocks
{
    public class VisibleBlock : MonoBehaviour
    {
        [Header("Visible Block Type")]
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
}