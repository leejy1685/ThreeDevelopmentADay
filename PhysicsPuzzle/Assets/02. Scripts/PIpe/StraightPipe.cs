using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightPipe : Pipe
{
    public Vector3 axis = Vector3.forward;

    public override void OnInteract()
    {

        if (!_isRotating)
            StartCoroutine(RotateSmoothStraight());
    }

    private IEnumerator RotateSmoothStraight()
    {
        _isRotating = true;

        // 바운딩 박스 중심 계산
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null) yield break;
        Vector3 center = rend.bounds.center;

        // 시작 위치,회전 저장
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // 목표 위치·회전 계산
        Quaternion deltaRot = Quaternion.AngleAxis(rotationAngle, axis.normalized);
        Vector3 offset = startPos - center;
        Vector3 endPos = center + deltaRot * offset;
        Quaternion endRot = deltaRot * startRot;

        // 시간 보간으로 위치·회전 설정
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 5) 최종 정확 보정
        transform.position = endPos;
        transform.rotation = endRot;
        _isRotating = false;
    }
}
