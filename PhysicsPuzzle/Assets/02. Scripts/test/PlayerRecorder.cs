using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRecorder : MonoBehaviour
{
    public List<Vector3> recordedPositions = new List<Vector3>(); // 녹화 시작시 플레이어의 위치를 담을 리스트
    public bool isRecording = false; // 기본은 녹화 안 함

    void Update()
    {
        // R 키를 눌러 녹화 on/off 토글
        if (Input.GetKeyDown(KeyCode.R)  )
        {
            isRecording = !isRecording;

            if (isRecording)
            {
                recordedPositions.Clear(); // 새로 시작할 땐 기존 기록 지움
                Debug.Log("녹화 시작");
            }
            else
            {
                Debug.Log("녹화 종료");
            }
        }

        // 녹화 중일 때 위치 기록
        if (isRecording)
        {
            recordedPositions.Add(transform.position);
        }
    }
}
