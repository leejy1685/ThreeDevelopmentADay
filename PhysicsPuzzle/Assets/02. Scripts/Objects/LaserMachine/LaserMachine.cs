
using _02._Scripts.Character.Player;
using _02._Scripts.Managers;
using _02._Scripts.Utils.Interface;
using System.Collections;
using UnityEngine;

namespace _02._Scripts.Objects.LaserMachine
{
    public enum LASER_COLOR
    {
        White,
        Blue,
        Green,
        Purple,
        Red,
        Yellow
    }
    public class LaserMachine : MonoBehaviour, IInteractable
    {
        [Header(" [ Components ] ")]
        [SerializeField] private Transform _pitchPivot;         // Z축 회전만 담당
        [SerializeField] private Transform body;
        [SerializeField] private LineRenderer _lineRenderer;      // 라인
        [SerializeField] public Transform barrel;   // <= 배럴 오브젝트 담당할듯...?
    
        //[SerializeField] public LASER_COLOR testColor;  //배럴 오브젝트에서 칼라정보 뽑아올것
        private GoalMachine _currentTarget = null;
    
    
        [Header(" [ Object Options ] ")]
        [SerializeField] private float _maxDistance = 100f; // 레이저 최대거리
        [SerializeField] private float _pitchSpeed = 60f;   // 상하 회전 속도
        [SerializeField] private float _maxRotate = 0f;     // 회전 최대각 (위 방향)
        [SerializeField] private float _minRotate = -90f;   // 회전 최소각 (앞 방향)

        [Header("Laser State")]
        [SerializeField] public LASER_COLOR laserColor = LASER_COLOR.Blue; // 현재 레이저 색상

        private bool _isFiring = false;  // 발사 체크 
        private float _currPitch;        // 현재 Z축 회전값
        private PlayerCondition _playerCondition;

        private void Awake()
        {
            //testColor = LASER_COLOR.Blue; // 테스트용 고정 초기값
            _currPitch = 0f;
        }

        private void Start()
        {
            _playerCondition = CharacterManager.Instance.Player.PlayerCondition;
        }


        void LateUpdate()
        {

            // 회전 제한(현재는 수직 ~ 수평)
            _currPitch = Mathf.Clamp(_currPitch, _minRotate, _maxRotate);
    
            // Z축 기준으로 회전
            _pitchPivot.localRotation = Quaternion.Euler(0f, 0f, _currPitch);
    
            UpdateLaser();
        }
    
        void UpdateLaser()
        {
            var direction = _pitchPivot.up;
            var startPosition = _pitchPivot.position + direction;
            var endPosition = startPosition + direction * _maxDistance;

    
            GoalMachine hitTarget = null;
    
            
            if (_isFiring)
            {
                // Raycast로 광선이 충돌하는 지점 검사함
                if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _maxDistance))
                {
                    endPosition = hit.point;

                    // 충돌 대상에 ILaserReceiver가 있으면 레이저 정보를 전달해서
                    if (hit.collider.TryGetComponent<ILaserReceiver>(out ILaserReceiver receiver))

                    {
                        // LaserBeam 구조체 생성 
                        LaserBeam beam = new LaserBeam(direction, laserColor);
                        receiver.OnLaserHit(beam);
                    }
                }
                // 캐시된 LineRenderer로 한 번만 설정
                // 기존 DrawLaserLine() 대신 사용 
                //->  LineRenderer lr = LinePoolManager.Instance.Get(); 프레임 단위 할당 방지
                _lineRenderer.enabled = true;
                _lineRenderer.startColor = _lineRenderer.endColor = new LaserBeam(direction, laserColor).ColorValue;
                _lineRenderer.SetPosition(0, startPosition);
                _lineRenderer.SetPosition(1, endPosition);

            }
            else
            {
                // 발사 중이 아니면 레이저 라인을 보여주지 않음
                // (라인 시작/끝을 동일하게 설정하여 길이 0인 선으로 처리하거나, 풀에 반환하여 제거)
            }


            // 레이가 벗어나서 현재의 타겟이 바뀔 경우
            if (_currentTarget && _currentTarget != hitTarget)
            {
                _currentTarget.CheckActive(LASER_COLOR.White);
            }
    
            _currentTarget = hitTarget;

        }

        /// <summary>
        /// 레이저 광선을 시각적으로 그려주는 메서드 (LineRenderer 풀 사용).
        /// </summary>
        /// <param name="start">레이저 시작 위치</param>
        /// <param name="end">레이저 끝 위치 (충돌 지점 또는 최대 사거리 지점)</param>
        /// <param name="colorType">레이저 색상 (LAZER_COLOR)</param>
        private void DrawLaserLine(Vector3 start, Vector3 end, LASER_COLOR colorType)
        {
            var lr = LinePoolManager.Instance.Get();   // 라인 풀에서 LineRenderer 하나 꺼냄
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            // 레이저 색상에 맞춰 라인 색상 설정
            var lineColor = LaserBeamColor(colorType);
            lr.startColor = lineColor;
            lr.endColor = lineColor;
            // 짧은 시간 후 라인을 풀로 반환 (한 프레임 동안만 표시)
            StartCoroutine(ReturnLineAfterFrame_Coroutine(lr));
        }
        
        /// <summary>
        /// 지정된 LineRenderer를 한 프레임 뒤 풀로 되돌립니다.
        /// </summary>
        private IEnumerator ReturnLineAfterFrame_Coroutine(LineRenderer lr)
        {
            // 매우 짧은 지연 후 라인 비활성화하여 다음 프레임에서 제거
            yield return new WaitForEndOfFrame();
            LinePoolManager.Instance.Return(lr);
        }

        /// <summary>
        /// 레이저 발사 Pitch 조정
        /// </summary>
        /// <param name="direction"></param>
        public void ControlLaserPitch(Vector2 direction)
        {
            _currPitch += direction.y * _pitchSpeed * Time.deltaTime;
        }
        
        
        public void ToggleLaser()
        {
            _isFiring = !_isFiring;
        }
        
        /// <summary>
        /// 배럴 != null 이라면, 배럴의 칼라값을 이용하여 SetLineColor(itemData.Color); <br/>
        /// Update에서 처리할 필요 없이 배럴을 끼고 빼고 할때 변경시키면 될듯<br/>
        /// private Color _razerColor 같이 레이저의 칼라 담당 변수 필요 <br/>
        /// 배럴의 색상에 따라 레이저 색이 결정된다.
        /// </summary>
        public void SetLineColor(LASER_COLOR color)
        {
            laserColor = color;
        }
        
        /// <summary>
        /// LAZER_COLOR를 실제 UnityEngine.Color로 변환합니다.
        /// </summary>
        private Color LaserBeamColor(LASER_COLOR colorType)
        {
            // LaserBeam.ColorValue 프로퍼티와 동일한 기능.
            switch (colorType)
            {
                case LASER_COLOR.Red: return Color.red;
                case LASER_COLOR.Blue: return Color.blue;
                case LASER_COLOR.Green: return Color.green;
                case LASER_COLOR.Purple: return Color.magenta;
                case LASER_COLOR.Yellow: return Color.yellow;
                case LASER_COLOR.White:
                default: return Color.white;
            }
        }
        
        /// <summary>
        /// 플레이어가 레이저 머신과 상호작용할 시 호출하는 함수
        /// 플레이어는 캐릭터 제어권을 상실하지만 대신 레이저 머신을 제어할 수 있게 된다.
        /// </summary>
        public void OnInteract()
        {
            _playerCondition.MigrateCameraFocusToOtherObject(body);
        }
        
    }
}
