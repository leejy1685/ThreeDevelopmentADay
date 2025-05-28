using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollower : MonoBehaviour
{
    public List<Vector3> pathToFollow = new List<Vector3>(); //플레이어의 녹화된 이동 경로를 받아올 유령 이동 경로 리스트
    public float moveSpeed = 12f; 
    private int _currentIndex = 0; // 시작 인덱스 (녹화 된 처음 위치부터 시작)

    public float del_Delay = 2f; // 이동 종료 후 사라지기까지의 대기 시간 (초)

    public void StartReplay(List<Vector3> path)
    {
        pathToFollow = path;
        _currentIndex = 0;
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while (_currentIndex < pathToFollow.Count)
        {
            Vector3 target = pathToFollow[_currentIndex];

            // 목적지까지 이동
            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            _currentIndex++;
            yield return null;
        }

        // 마지막 지점 도달 후 딜레이만큼 대기
        yield return new WaitForSeconds(del_Delay);

        // 유령 오브젝트 삭제
        Destroy(gameObject);
    }
}
