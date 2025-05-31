using System.Collections;
using _02._Scripts.Character.Player;
using UnityEngine;

namespace _02._Scripts.Objects
{
    public class MoveBlock : MonoBehaviour
    {
        public enum MOVEMENTAXIS
        {
            LEFT_RIGHT_X, // 옵션 1: 좌우 (X축)
            FORWARD_BACKWARD_Z, // 옵션 2: 앞뒤 (Z축)
            UP_DOWN_Y // 옵션 3: 위아래 (Y축)
        }

        [Header("Movement Settings")] 
        public MOVEMENTAXIS moveDirection;
        public float moveSpeed = 2f; // 블록의 스피드
        public float moveDistance = 3f; // 블록의 움직임의 폭

        [Header("Player Interaction")] [SerializeField]
        private LayerMask _playerLayer; // Player레이어 선택하기


        private Vector3 _startPosition;
        private Transform _player;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            // 1. 플레이어 찾기
            _player = FindFirstObjectByType<PlayerController>()?.transform;

            // 2. 플레이어를 못 찾았을 경우 경고 메시지 출력
            if (_player == null)
            {
                Debug.LogError("PlayerController를 찾을 수 없습니다!");
            }

            // 3. Rigidbody 가져오기

            _rigidbody = GetComponent<Rigidbody>();

            // 4. Rigidbody 설정
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        void Start()
        {
            // 현재 블록의 위치를 _startPosition 변수에 저장
            _startPosition = transform.position;

            StartCoroutine(MovePlatformCoroutine());
        }

        IEnumerator MovePlatformCoroutine()
        {
            while (true)
            {
                // 1. 움직일 양 계산

                float moveAmount = Mathf.Sin(Time.fixedTime * moveSpeed) * moveDistance;

                // 2. 새로운 위치 계산
                //일단 시작 위치를 기준으로잡기
                Vector3 newPos = _startPosition;

                // 3. 선택된 방향으로 움직임 적용
                switch (moveDirection)
                {
                    case MOVEMENTAXIS.LEFT_RIGHT_X:
                        newPos.x += moveAmount;
                        break;

                    case MOVEMENTAXIS.FORWARD_BACKWARD_Z:
                        newPos.z += moveAmount;
                        break;

                    case MOVEMENTAXIS.UP_DOWN_Y:
                        newPos.y += moveAmount;
                        break;
                }

                // 4. 블록 이동 실행


                _rigidbody.MovePosition(newPos);

                // 5. 다음 물리 업데이트까지 대기

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision, true);
        }

        private void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision, true);
        }

        private void OnCollisionExit(Collision collision)
        {
            HandleCollision(collision, false);
        }

        // HandleCollision: 실제 충돌 처리 로직을 담고 있는 함수

        private void HandleCollision(Collision collision, bool isColliding)
        {
            // 1. 플레이어와 부딪혔는지 확인:

            if (_player != null && (_playerLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                // 2. 플랫폼 위에 있는지 확인
                bool isOnTop = false;
                if (isColliding)
                {
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        // contact.normal  부딪힌 지점의 '법선 벡터' (표면에서 수직으로 튀어나오는 방향)

                        if (contact.normal.y > 0.5f)
                        {
                            isOnTop = true;
                            break;
                        }
                    }
                }


                // 3. 부모-자식 관계 설정 또는 해제
                if (isColliding)
                {
                    // 만약 플레이어가 플랫폼 '위'에 있고, 아직 이 플랫폼의 '자식'이 아니라면,
                    if (isOnTop && _player.parent != transform)
                    {
                        Debug.Log("플레이어 부모 설정!"); // 잘 되는지 확인용
                        // 플레이어를 이 플랫폼의 자식으로 만든다
                        // > 이렇게 하면 플랫폼이 움직일 때 플레이어도 자동으로 따라 움직임
                        _player.SetParent(transform);
                    }
                    // 만약 플레이어가 플랫폼 '위'에 있지 않거나 옆에 있는데, '자식'으로 되어 있다면,
                    else if (!isOnTop && _player.parent == transform)
                    {
                        Debug.Log("플레이어 부모 해제 (옆면)!");
                        //  플레이어의 부모를 없애서(null) 최상위 계층으로 보냄 (더 이상 안 따라 움직임)
                        _player.SetParent(null);
                    }
                }
                else
                {
                    // 만약 플레이어가 이 플랫폼의 '자식'으로 되어 있다면,
                    if (_player.parent == transform)
                    {
                        Debug.Log("플레이어 부모 해제 (Exit)!"); // 콘솔에 메시지 출력
                        // 무조건 부모-자식 관계를 해제합
                        _player.SetParent(null);
                    }
                }
            }
        }
    }
}