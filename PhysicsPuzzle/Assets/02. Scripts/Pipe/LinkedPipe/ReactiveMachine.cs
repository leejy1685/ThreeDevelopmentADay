using System.Collections.Generic;
using _02._Scripts.Character.Player;
using _02._Scripts.Character.Player.Interface;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Pipe.Common.Interface;
using _02._Scripts.Pipe.LaserPipe;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02._Scripts.Pipe.LinkedPipe
{
    public class ReactiveMachine : MonoBehaviour, IInteractable
    {
        [Header(" [ Components ] ")]
        [SerializeField] private Transform _pitchPivot;         // Z축 회전만 담당
        [SerializeField] private LineRenderer _lineRenderer;      // 라인
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _pillar;

        [Header(" [ Body Textures ] ")] 
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private List<Material> materials;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Material> materialDictionary;

        [Header(" [ Object Options ] ")]
        [SerializeField] private float _maxDistance = 100f; // 레이저 최대거리
        [SerializeField] private float _pitchSpeed = 60f;   // 상하 회전 속도
        [SerializeField] private float _maxRotate = 0f;     // 회전 최대각 (위 방향)
        [SerializeField] private float _minRotate = -90f;   // 회전 최소각 (앞 방향)

        [Header("Laser State")]
        [SerializeField] public LASER_COLOR laserColor = LASER_COLOR.Blue; // 현재 레이저 색상

        [Header("Sound Source")]
        [SerializeField] private AudioClip _interactionSound;
        
        // Fields
        private GoalMachine _currentTarget = null;
        private bool _isFiring;  // 발사 체크 
        private float _currPitch;        // 현재 Z축 회전값
        private PlayerCondition _playerCondition;
        
        private void Awake()
        {
            var i = 0;
            foreach (var material in materials) materialDictionary.TryAdd((LASER_COLOR)i++, material);
        }

        private void Start()
        {
            _currPitch = 0f;
            _playerCondition = CharacterManager.Instance.Player.PlayerCondition;
        }

        private void LateUpdate()
        {
            // 회전 제한(현재는 수직 ~ 수평)
            _currPitch = Mathf.Clamp(_currPitch, _minRotate, _maxRotate);

            // Z축 기준으로 회전
            _pitchPivot.localRotation = Quaternion.Euler(0f, 0f, _currPitch);

            UpdateLaser();
        }

        private void UpdateLaser()
        {
            var direction = _pitchPivot.up;
            var startPosition = _pitchPivot.position + direction;
            var endPosition = startPosition + direction * _maxDistance;

            if (_isFiring)
            {
                // Raycast로 광선이 충돌하는 지점 검사함
                if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _maxDistance))
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
                _lineRenderer.enabled = false;
            }
        }
        
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
            // Debug.Log($"색 체크 : {laserColor}");

            foreach (var render in renderers)
            {
                render.material = materialDictionary[color];
            }
        }

        public void OnInteract()
        {
            SoundManager.PlaySfx(_interactionSound);
            _playerCondition.MigrateCameraFocusToOtherObject(_body);
        }
    }
}