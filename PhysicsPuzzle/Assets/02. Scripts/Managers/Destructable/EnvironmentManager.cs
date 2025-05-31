using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        // Components
        [SerializeField] private Environment.DayAndNight dayAndNight;
        
        // Properties
        public Environment.DayAndNight DayAndNight => dayAndNight;
    }
}