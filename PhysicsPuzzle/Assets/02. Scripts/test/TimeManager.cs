using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeState 
    { 
        Day,
        Night 
    }
    public TimeState currentTime;

    public GameObject ghostPrefab; //유렴 플레이어 넣으면 됩니다.
    public PlayerRecorder playerRecorder;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeTime();
        }

        // 밤일 때만 G 키로 유령 소환
        if (currentTime == TimeState.Night && Input.GetKeyDown(KeyCode.G))
        {
            SpawnGhost();
        }
    }

    void ChangeTime()
    {
        if (currentTime == TimeState.Day)
        {
            currentTime = TimeState.Night;
            Debug.Log("밤");
        }
        else
        {
            currentTime = TimeState.Day;
            Debug.Log("낮");
        }
    }

    void SpawnGhost()
    {
        if (playerRecorder.recordedPositions.Count > 0)
        {
            // 유령 생성 및 경로 재생 시작
            GameObject ghost = Instantiate(
                ghostPrefab,
                playerRecorder.recordedPositions[0],
                Quaternion.identity
            );

            GhostFollower gf = ghost.GetComponent<GhostFollower>();
            gf.StartReplay(new System.Collections.Generic.List<Vector3>(playerRecorder.recordedPositions));
        }
        else
        {
            Debug.Log("경로가 녹화되지 않았습니다");
        }
    }
}
