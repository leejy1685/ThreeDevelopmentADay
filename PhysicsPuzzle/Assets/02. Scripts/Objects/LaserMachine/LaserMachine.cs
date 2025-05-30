using _02._Scripts.Character.Player;
using _02._Scripts.Managers;
using _02._Scripts.Utils.Interface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        [SerializeField] private Transform pitchPivot; // Z축 회전만 담당
        [SerializeField] private Transform body;
        [SerializeField] private LineRenderer lineRenderer; // 라인
        [SerializeField] private Barrel barrel;
        
        [Header(" [ Body Textures ] ")] 
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private List<Material> materials;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Material> materialDictionary;
        
        [Header(" [ Object Options ] ")] 
        [SerializeField] private float maxDistance = 100f; // 레이저 최대거리
        [SerializeField] private float pitchSpeed = 30f; // 상하 회전 속도
        [SerializeField] private float maxRotate = 0f; // 회전 최대각 (위 방향)
        [SerializeField] private float minRotate = -90f; // 회전 최소각 (앞 방향)

        [Header(" [ Laser State ] ")] 
        [SerializeField] public LASER_COLOR laserColor = LASER_COLOR.Blue; // 현재 레이저 색상

        private GoalMachine _currentTarget = null;
        private PlayerCondition _playerCondition;
        private bool _isFiring = false; // 발사 체크 
        private float _currPitch; // 현재 Z축 회전값

        private void Awake()
        {
            var i = 0;
            foreach (var material in materials) materialDictionary.TryAdd((LASER_COLOR)i++, material);
        }

        private void Start()
        {
            _currPitch = 0f;
            _playerCondition = CharacterManager.Instance.Player.PlayerCondition;
            barrel.Init(this);
            SetColorOfMachine(laserColor);
        }
        
        private void LateUpdate()
        {
            // 회전 제한(현재는 수직 ~ 수평)
            _currPitch = Mathf.Clamp(_currPitch, minRotate, maxRotate);

            // Z축 기준으로 회전
            pitchPivot.localRotation = Quaternion.Euler(0f, 0f, _currPitch);

            UpdateLaser();
        }

        void UpdateLaser()
        {
            var direction = pitchPivot.up;
            var startPosition = pitchPivot.position + direction;
            var endPosition = startPosition + direction * maxDistance;
            
            GoalMachine hitTarget = null;
            if (_isFiring)
            {
                // Raycast로 광선이 충돌하는 지점 검사함
                if (Physics.Raycast(startPosition, direction, out var hit, maxDistance))
                {
                    endPosition = hit.point;

                    // 충돌 대상에 ILaserReceiver가 있으면 레이저 정보를 전달해서
                    if (hit.collider.TryGetComponent(out ILaserReceiver receiver))

                    {
                        // LaserBeam 구조체 생성 
                        LaserBeam beam = new LaserBeam(direction, laserColor);
                        receiver.OnLaserHit(beam);
                    }
                }

                // 캐시된 LineRenderer로 한 번만 설정
                // 기존 DrawLaserLine() 대신 사용
                lineRenderer.enabled = true;
                lineRenderer.startColor = lineRenderer.endColor = new LaserBeam(direction, laserColor).ColorValue;
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPosition);
            }
            else
            {
                // 발사 중이 아니면 레이저 라인을 보여주지 않음
                // (라인 시작/끝을 동일하게 설정하여 길이 0인 선으로 처리하거나, 풀에 반환하여 제거)
                lineRenderer.enabled = false;
            }


            // 레이가 벗어나서 현재의 타겟이 바뀔 경우
            if (_currentTarget && _currentTarget != hitTarget)
            {
                _currentTarget.CheckActive(LASER_COLOR.White);
            }

            _currentTarget = hitTarget;
        }

        /// <summary>
        /// 레이저 발사 Pitch 조정
        /// </summary>
        /// <param name="direction"></param>
        public void ControlLaserPitch(Vector2 direction)
        {
            _currPitch += direction.y * pitchSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Toggle 방식으로 레이저를 발사
        /// </summary>
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
        public void SetColorOfMachine(LASER_COLOR color)
        {
            laserColor = color;
            foreach (var render in renderers)
            {
                render.material = materialDictionary[color];
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