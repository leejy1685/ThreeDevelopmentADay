using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LobbyCamera : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    private CinemachineSmoothPath path;
    private Rigidbody _rigidbody;
    

    private void Awake()
    {
        path = FindAnyObjectByType<CinemachineSmoothPath>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //Camera.main.cullingMask += _playerLayer;
        StartCoroutine(CameraMove_Coroutine());
    }

    public void DisableCamera()
    {
        Camera.main.cullingMask -= _playerLayer;
        gameObject.SetActive(false);
    }

    
    
    private IEnumerator CameraMove_Coroutine()
    {
        int waypointCount = path.m_Waypoints.Length;

        int i = 0;
        
        //루프 형태 일 때
        while(path.Looped)
        {
            Vector3 startPos = path.transform.TransformPoint(path.m_Waypoints[i % waypointCount].position);
            Vector3 endPos = path.transform.TransformPoint(path.m_Waypoints[(i + 1) % waypointCount].position);

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
        while (!path.Looped)
        {
            int currentIndex = (int)Mathf.PingPong(i, waypointCount - 1);;
            int nextIndex = (int)Mathf.PingPong(i+1, waypointCount - 1);
            
            Vector3 startPos = path.transform.TransformPoint(path.m_Waypoints[currentIndex].position);
            Vector3 endPos = path.transform.TransformPoint(path.m_Waypoints[nextIndex].position);

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
