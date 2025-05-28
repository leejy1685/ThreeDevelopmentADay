using _02._Scripts.Character.Player;
using _02._Scripts.Managers;
using _02._Scripts.Utils.Interface;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
            // #########플레이어 세팅 완료되면 주석 풀것##########
            //_playerCondition = CharacterManager.Instance.Player.PlayerCondition;
        }

        void LateUpdate()
        {
            // 발사 토글(일단은 좌 컨트롤) => LazerToggle 호출시에 지워도 됨
            if (Input.GetKeyDown(KeyCode.LeftControl))
                _isFiring = !_isFiring;
            TestControl();


            // 회전 제한(현재는 수직 ~ 수평)
            _currPitch = Mathf.Clamp(_currPitch, _minRotate, _maxRotate);

            // Z축 기준으로 회전
            _pitchPivot.localRotation = Quaternion.Euler(0f, 0f, _currPitch);

            if (_isFiring)
                UpdateLaser();
            else
                _lineRenderer.enabled = false;
        }

        void UpdateLaser()
        {
            Vector3 direction = _pitchPivot.up;
            Vector3 startPosition = _pitchPivot.position + direction;
            Vector3 endPosition = startPosition + direction * _maxDistance;

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
        }

        public void ControlLaserPitch(Vector2 direction)
        {
            _currPitch += direction.y * _pitchSpeed * Time.deltaTime;
        }
        public void ToggleLaser()
        {
            _isFiring = !_isFiring;
            //if(_isFiring){LineRenderer lr = LinePoolManager.Instance.Get();}
            // else{ LinePoolManager.Instance.Return(); }
        }
        /// <summary>
        /// 배럴 != null 이라면, 배럴의 칼라값을 이용하여 SetLineColor(itemData.Color); <br/>
        /// Update에서 처리할 필요 없이 배럴을 끼고 빼고 할때 변경시키면 될듯<br/>
        /// private Color _razerColor 같이 레이저의 칼라 담당 변수 필요 <br/>
        /// 배럴의 색상에 따라 레이저 색이 결정된다.
        /// </summary>
        public void SetLineColor(LASER_COLOR color)
        {
            // 여기서 
            var lazerColor = color switch
            {
                LASER_COLOR.White => Color.white,
                LASER_COLOR.Blue => Color.blue,
                LASER_COLOR.Green => Color.green,
                LASER_COLOR.Purple => Color.magenta,
                LASER_COLOR.Red => Color.red,
                LASER_COLOR.Yellow => Color.yellow,
                _ => new Color()
            };
            _lineRenderer.startColor = lazerColor;
            _lineRenderer.endColor = lazerColor;
        }
    
        public void OnInteract()
        {
            //_playerCondition.MigrateCameraFocusToOtherObject(body);
        }



        public void TestControl() // (테스트용)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                _currPitch += _pitchSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                _currPitch -= _pitchSpeed * Time.deltaTime;
        }
    }
}
