using System;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers;
using _02._Scripts.Utils.Interface;
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
    
        [SerializeField] public LASER_COLOR testColor;  //배럴 오브젝트에서 칼라정보 뽑아올것
        private GoalMachine _currentTarget = null;
    
    
        [Header(" [ Object Options ] ")]
        [SerializeField] private float _maxDistance = 100f; // 레이저 최대거리
        [SerializeField] private float _pitchSpeed = 60f;   // 상하 회전 속도
        [SerializeField] private float _maxRotate = 0f;     // 회전 최대각 (위 방향)
        [SerializeField] private float _minRotate = -90f;   // 회전 최소각 (앞 방향)
        
        private bool _isFiring = false;  // 발사 체크 
        private float _currPitch;        // 현재 Z축 회전값
        private PlayerCondition _playerCondition;
    
        private void Awake()
        {
            _currPitch = 0f;
        }

        private void Start()
        {
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
            Vector3 direction = _pitchPivot.up;
            Vector3 startPosition = _pitchPivot.position + direction;
            Vector3 endPosition = startPosition + direction * _maxDistance;
    
            GoalMachine hitTarget = null;
    
            if (_isFiring)
            {
                // 일단은 goalObject의 레이어 체크는 안했습니다.
                if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _maxDistance))
                {
                    endPosition = hit.point;
                    if (hit.collider.TryGetComponent<GoalMachine>(out var target))
                    {
                        hitTarget = target;
                        hitTarget.CheckActive(testColor);
                    }
                }
    
                _lineRenderer.SetPosition(0, startPosition);
                _lineRenderer.SetPosition(1, endPosition);
            }
            else
            {
                _lineRenderer.SetPosition(0, startPosition);
                _lineRenderer.SetPosition(1, startPosition);
            }
    
            // 레이가 벗어나서 현재의 타겟이 바뀔 경우
            if (_currentTarget && _currentTarget != hitTarget)
            {
                _currentTarget.CheckActive(LASER_COLOR.White);
            }
    
            _currentTarget = hitTarget;
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
            _playerCondition.MigrateCameraFocusToOtherObject(body);
        }
    }
}
