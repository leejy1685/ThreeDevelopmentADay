using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        // Components
        [SerializeField] private DayAndNight dayAndNight;
        
        // Properties
        public DayAndNight DayAndNight => dayAndNight;
    }
}