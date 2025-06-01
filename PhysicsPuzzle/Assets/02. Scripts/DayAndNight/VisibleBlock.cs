using _02._Scripts.Environment;
using UnityEngine;

namespace _02._Scripts.DayAndNight
{
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
}
