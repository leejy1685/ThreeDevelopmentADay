using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// 레이저를 전달하고 굴절시키는 파이프 클래스.
/// PipeType는 구현하다보니, 포트 중심으로 발사하기에 필요가 없네요
/// </summary>
public class Pipe : MonoBehaviour, IInteractable
{
    [Header("회전 설정")]
    public float rotationAngle = 90f;
    public float rotationDuration = 0.5f;

    [Header("레이저 최대 사거리")]
    public float maxRayDistance = 10f;

    [Header("라인 렌더러 프리팹")]
    [SerializeField]private LineRenderer lineRenderer;

    [Header("[파이브 몸체 파티클]")]
    [SerializeField] private PipeBodyParticleSystem Body;
    

    private List<Port> ports;  // 이 파이프에 포함된 모든 Port들
    protected bool isRotating;

    // 포트별로 캐싱된 LineRenderer
    private Dictionary<Port, LineRenderer> portLineMap;
    // 포트별로 마지막으로 레이저를 발사한 프레임 기록
    private Dictionary<Port, int> lastShotFrameByPort;

    void Awake()
    {
        ports = GetComponentsInChildren<Port>().ToList();
        portLineMap = new Dictionary<Port, LineRenderer>();
        lastShotFrameByPort = new Dictionary<Port, int>();

        // 포트마다 LineRenderer 한 개씩 생성·캐싱
        foreach (var p in ports)
        {
            p.parentPipe = this;
            lastShotFrameByPort[p] = -1;

            // 프리팹에서 복제
            var go = Instantiate(lineRenderer, transform);
            go.name = $"{name}_Line_for_{p.name}";
            var lr = go.GetComponent<LineRenderer>();
            // 기본 설정
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.enabled = false;
            portLineMap[p] = lr;
        }
    }

    //private void Update()
    //{
    //    //Pipe 회전 테스트용
    //    //각 파이프에 따라 회전에 대해서 상속 구조 고려
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        OnInteract();
    //    }
    //}

    private void LateUpdate()
    {
        // 포트별로, 마지막 사용 프레임이 지났다면 라인 끄기
        foreach (var kv in portLineMap)
        {
            var port = kv.Key;
            var lr = kv.Value;
            if (Time.frameCount > lastShotFrameByPort[port])
            {
                lr.enabled = false;
                port.StopEffect(); //파티클 정지
                Body.StopBodyEffect();


            }

        }
    }
    public virtual void OnInteract(){}
 
    /// <summary>
    /// 특정 Port로 레이저가 들어왔을 때 호출됩니다. 해당 포트를 제외한 나머지 포트들로 레이저를 발사
    /// </summary>
    /// <param name="hitPort">레이저가 들어온 Port</param>
    /// <param name="beam">수신한 LaserBeam 정보 (방향, 색상)</param>
    public void OnPortHit(Port hitPort, LaserBeam beam)
    {
        // 같은 프레임에 중복 처리 방지
        if (lastShotFrameByPort[hitPort] == Time.frameCount) return;
        lastShotFrameByPort[hitPort] = Time.frameCount;

        // 입력된 포트 제외하고 나머지 포트들로 레이저 분기
        for (int i = 0; i < ports.Count; i++)
        {
            var outPort = ports[i];
            if (outPort == hitPort) continue;
            ShootFromPort(outPort, beam);
        }
    }

    /// <summary>
    /// 주어진 포트로부터 지정된 레이저를 발사합니다. 레이저가 진행되어 닿는 대상의 ILaserReceiver를 호출
    /// </summary>
    /// <param name="port">출력용 Port</param>
    /// <param name="incomingBeam">들어온 레이저 (방향, 색상 정보)</param>
    private void ShootFromPort(Port port, LaserBeam incomingBeam)
    {
        var lr = portLineMap[port];
        var origin = port.transform.position;
        var dir = port.WorldExitDirection;
        var end = origin + dir * maxRayDistance;

        // Raycast 충돌 검사
        if (Physics.Raycast(origin, dir, out RaycastHit hit, maxRayDistance))
        {
            end = hit.point;
            if (hit.collider.TryGetComponent<ILaserReceiver>(out var recv))
                recv.OnLaserHit(new LaserBeam(dir, incomingBeam.colorType));
        }

        // 이 포트 라인에 색상·위치 설정 후 켜기
        Color c = incomingBeam.ColorValue;
        lr.startColor = lr.endColor = c;
        lr.SetPosition(0, origin);
        lr.SetPosition(1, end);
        lr.enabled = true;

        // 이 포트 라인을 이번 프레임에 사용했음을 기록
        lastShotFrameByPort[port] = Time.frameCount;
    }

    /// <summary>
    /// 파이프 회전 코루틴
    /// Pivot y축 기준으로 회전하는 파이프는 이 메소드 사용(Elboe,Cross)
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    public IEnumerator RotateAroundPivot(Vector3 axis)
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(axis * rotationAngle);

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
        isRotating = false;
    }
}
