using System.Collections;
using Cinemachine;
using UnityEngine;

namespace _02._Scripts.Camera
{
    public class LobbyCamera : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        private CinemachineSmoothPath _path;
        private Rigidbody _rigidbody;
    

        private void Awake()
        {
            _path = FindAnyObjectByType<CinemachineSmoothPath>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            UnityEngine.Camera.main.cullingMask += playerLayer;
            StartCoroutine(CameraMove_Coroutine());
        }

        public void DisableCamera()
        {
            UnityEngine.Camera.main.cullingMask -= playerLayer;
            gameObject.SetActive(false);
        }

    
    
        private IEnumerator CameraMove_Coroutine()
        {
            int waypointCount = _path.m_Waypoints.Length;

            int i = 0;
        
            //루프 형태 일 때
            while(_path.Looped)
            {
                Vector3 startPos = _path.transform.TransformPoint(_path.m_Waypoints[i % waypointCount].position);
                Vector3 endPos = _path.transform.TransformPoint(_path.m_Waypoints[(i + 1) % waypointCount].position);

                int totalFrames = 60;

                for (int frame = 0; frame < totalFrames; frame++)
                {
                    float t = (float)(frame + 1) / totalFrames;
                    Vector3 nextPos = Vector3.Lerp(startPos, endPos, t);

                    // Rigidbody로 이동 (관성 제거 방식)
                    _rigidbody.MovePosition(nextPos);

                    yield return new WaitForFixedUpdate(); // Rigidbody 물리 이동은 FixedUpdate 기준
                }

                i++;
            }

            //루피 형태가 아닐 때
            while (!_path.Looped)
            {
                int currentIndex = (int)Mathf.PingPong(i, waypointCount - 1);;
                int nextIndex = (int)Mathf.PingPong(i+1, waypointCount - 1);
            
                Vector3 startPos = _path.transform.TransformPoint(_path.m_Waypoints[currentIndex].position);
                Vector3 endPos = _path.transform.TransformPoint(_path.m_Waypoints[nextIndex].position);

                int totalFrames = 60;

                for (int frame = 0; frame < totalFrames; frame++)
                {
                    float t = (float)(frame + 1) / totalFrames;
                    Vector3 nextPos = Vector3.Lerp(startPos, endPos, t);

                    // Rigidbody로 이동 (관성 제거 방식)
                    _rigidbody.MovePosition(nextPos);

                    yield return new WaitForFixedUpdate(); // Rigidbody 물리 이동은 FixedUpdate 기준
                }

                i++;
            }
        }
    }
}
