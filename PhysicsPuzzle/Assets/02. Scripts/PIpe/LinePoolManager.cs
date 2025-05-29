using System.Collections.Generic;
using UnityEngine;

public class LinePoolManager : MonoBehaviour
{
    public static LinePoolManager Instance { get; private set; }

    [Header("Pool에 사용할 LineRenderer 프리팹")]
    [SerializeField] private GameObject linePrefab;

    [Header("초기 풀 크기")]
    [SerializeField] private int initialSize = 10;

    // 사용 가능한(비활성) 풀
    private readonly Queue<LineRenderer> _pool = new Queue<LineRenderer>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 미리 여러 개 생성해 두기
        for (int i = 0; i < initialSize; i++)
        {
            var go = Instantiate(linePrefab, transform);
            go.SetActive(false);
            _pool.Enqueue(go.GetComponent<LineRenderer>());
        }
    }

    /// <summary>
    /// 풀에서 하나 꺼내거나, 비어 있으면 새로 인스턴스화
    /// </summary>
    public LineRenderer Get()
    {
        LineRenderer lr;
        if (_pool.Count > 0)
        {
            lr = _pool.Dequeue();
        }
        else
        {
            var go = Instantiate(linePrefab, transform);
            lr = go.GetComponent<LineRenderer>();
        }

        lr.gameObject.SetActive(true);
        return lr;
    }

    /// <summary>
    /// 사용이 끝난 LineRenderer를 풀로 돌려보냄
    /// </summary>
    public void Return(LineRenderer lr)
    {
        lr.positionCount = 0;
        lr.gameObject.SetActive(false);
        _pool.Enqueue(lr);
    }
}
