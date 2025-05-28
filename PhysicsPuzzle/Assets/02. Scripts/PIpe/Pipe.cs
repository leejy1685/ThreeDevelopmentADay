using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<Port> ports;  // 이 파이프에 포함된 모든 Port들
    protected bool isRotating;

    private void Awake()
    {
        // 자식 오브젝트들에서 Port 컴포넌트 찾아 리스트 초기화
        ports = GetComponentsInChildren<Port>().ToList();
        // 각 Port에 자신의 parentPipe 설정
        foreach (Port p in ports)
            p.parentPipe = this;
    }

    private void Update()
    {
        //Pipe 회전 테스트용
        //각 파이프에 따라 회전에 대해서 상속 구조 고려
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnInteract();
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
        IEnumerable<Port> outputPorts = ports.Where(p => p != hitPort);
        foreach (Port outPort in outputPorts)
        {
            // 입력받은 레이저 정보를 사용해 각 출력 포트로 레이저 발사
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
        Vector3 origin = port.transform.position;       // 레이저 발사 원점 (포트 위치)
        Vector3 dir = port.WorldExitDirection;          // 레이저 발사 방향 (포트의 출구 방향, 월드 기준)
        LaserBeam outBeam = new LaserBeam(dir, incomingBeam.colorType);  // 출력 레이저 정보 (방향 갱신, 색상 유지)

        // 레이캐스트로 진행 경로 상의 충돌 검사
        Vector3 targetPos = origin + dir * maxRayDistance;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, maxRayDistance))
        {
            targetPos = hit.point;
            // 충돌한 객체에 ILaserReceiver 구현이 있으면 레이저 전달
            if (hit.collider.TryGetComponent<ILaserReceiver>(out ILaserReceiver receiver))
            {
                receiver.OnLaserHit(outBeam);
            }
        }
        // 레이저 경로를 시각적으로 표시 (라인 렌더러 생성 및 설정)
        DrawLaserLine(origin, targetPos, incomingBeam.colorType);
    }

    /// <summary>
    /// 레이저 광선을 시각적으로 표시하기 위해 LineRenderer를 풀에서 가져와 설정
    /// </summary>
    private void DrawLaserLine(Vector3 start, Vector3 end, LASER_COLOR colorType)
    {
        LineRenderer lr = LinePoolManager.Instance.Get();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        // LaserBeam 색상에 맞춰 라인 색상 지정
        Color lineColor = LaserBeamColor(colorType);
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        // 한 프레임 뒤 라인을 풀로 반환하여 잔상 제거
        StartCoroutine(ReturnLineAfterFrame(lr));
    }

    /// <summary>
    /// LAZER_COLOR 값을 실제 Color로 변환)
    /// </summary>
    private Color LaserBeamColor(LASER_COLOR colorType)
    {
        switch (colorType)
        {
            case LASER_COLOR.Red: return Color.red;
            case LASER_COLOR.Blue: return Color.blue;
            case LASER_COLOR.Green: return Color.green;
            case LASER_COLOR.Purple: return Color.magenta;
            case LASER_COLOR.Yellow: return Color.yellow;
            case LASER_COLOR.White:
            default: return Color.white;
        }
    }

    /// <summary>
    /// LineRenderer를 한 프레임 뒤에 풀로 반환
    /// </summary>
    private IEnumerator ReturnLineAfterFrame(LineRenderer lr)
    {
        yield return new WaitForEndOfFrame();
        LinePoolManager.Instance.Return(lr);
    }
}
