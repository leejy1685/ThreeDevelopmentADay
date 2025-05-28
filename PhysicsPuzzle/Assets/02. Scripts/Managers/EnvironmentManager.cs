using System;
using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.Managers
{
    public class EnvironmentManager : MonoBehaviour
    {
        // Components
        [SerializeField] private DayAndNight dayAndNight;
        
        // Properties
        public DayAndNight DayAndNight => dayAndNight;
        
        // Singleton
        public static EnvironmentManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance) { Instance = this; } 
            else {if(Instance != this) Destroy(gameObject);}
        }
    }
}