using UnityEngine;

namespace _02._Scripts.Objects.LaserMachine
{
    public class GoalMachine : MonoBehaviour
    {
        [SerializeField] private Transform _body;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _isActivate;
        [SerializeField] private LASER_COLOR _color;
    
        void Awake()
        {
            _isActivate = false;
            _color = LASER_COLOR.Blue;
            // 접근 시 자동으로 머티리얼 인스턴스를 복제
            _renderer.material.EnableKeyword("_EMISSION");
        }
    
    
        public void CheckActive(LASER_COLOR color)
        {
            if (color == _color) { OnLight(); }
            else { OffLight(); }
        }
    
        private void OnLight()
        {
            _isActivate = true;
            SetEmissionColor(Color.white);
        }
        private void OffLight()
        {
            _isActivate = false;
            SetEmissionColor(Color.black);
        }
        private void SetEmissionColor(Color color)
        {
            var mat = _renderer.material; 
            mat.SetColor("_EmissionColor", color);
        }
    }
}
