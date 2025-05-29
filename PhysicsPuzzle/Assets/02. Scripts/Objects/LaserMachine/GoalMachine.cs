using UnityEngine;

namespace _02._Scripts.Objects.LaserMachine
{
    public class GoalMachine : MonoBehaviour, ILaserReceiver
    {
        [SerializeField] private Transform _body;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _isActivate;
        [SerializeField] private LASER_COLOR _color;


        private float _lastHitTime;
        private float _emissionDuration = 0.05f;

        void Awake()
        {
            OffLight();
            // 색깔별로 그냥 프리팹 만들어서 써도 무관할 듯 싶습니다.
            _color = LASER_COLOR.Blue;
            // 접근 시 자동으로 머티리얼 인스턴스를 복제
            _renderer.material.EnableKeyword("_EMISSION");
        }
        private void Update()
        {
            if (_isActivate && Time.time - _lastHitTime > _emissionDuration)
            {
                OffLight(); // 너무 오래 레이저가 안 들어오면 자동 off
            }
        }

        public void CheckActive(LASER_COLOR color)
        {
            if (color == _color)
            {
                _lastHitTime = Time.time;
                OnLight(); // 지속 갱신
                StageManager.Instance.IncreaseActiveCount();
            }
            else
            {
                OffLight();
                StageManager.Instance.DecreaseActiveCount();
            }
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

        public void OnLaserHit(LaserBeam beam)
        {
            CheckActive(beam.colorType);
        }
    }
}
