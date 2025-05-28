using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.Objects.Blocks
{
    public class CollisionBlock : MonoBehaviour
    {
        [SerializeField] private TIME_TYPE timeBlockType;
        
        public void ChangeSetActive(TIME_TYPE isDay)
        {
            if (isDay == timeBlockType) gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}