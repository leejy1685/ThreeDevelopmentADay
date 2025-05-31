using UnityEngine;
using _02._Scripts.Objects.LaserMachine;

[RequireComponent(typeof(Collider))]
/// <summary>
/// 파이프의 한 쪽 끝 단자를 나타내는 포트 클래스.
/// 레이저를 수신하면 자신이 속한 Pipe에 전달
/// </summary>
public class Port : MonoBehaviour, ILaserReceiver
{
    [HideInInspector] public Pipe parentPipe;

    [Header("이 포트의 로컬 기준 배출 방향")]
    public Vector3 exitLocalDirection = Vector3.up;

    [Header("파티클 효과")]
    [SerializeField] private ParticleSystem portEffect;

    [SerializeField]private PipeBodyParticleSystem PipeBodyParticleSystem;

    private void Awake()
    {
        // 시작 시 자신의 parentPipe 설정 (부모 객체의 Pipe 컴포넌트)
        if (parentPipe == null)
        {
            parentPipe = GetComponentInParent<Pipe>();
        }
    }

    private void Reset()
    {
        // Port 객체의 Collider는 Trigger로 설정
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    /// <summary>
    /// LaserMachine이나 다른 Pipe에서 레이저를 수신했을 때 호출
    /// </summary>
    /// <param name="beam">수신한 LaserBeam (방향, 색상)</param>
    public void OnLaserHit(LaserBeam beam)
    {
        //Debug.Log($"[Port] {name} 수신: 방향={beam.direction}, 색상={beam.colorType}");
        EmitEffect(beam.ColorValue);
        PipeBodyParticleSystem.BodyEffect(beam.ColorValue);
        // 자신이 속한 파이프에 이 포트로 레이저가 들어왔음을 알림
        parentPipe.OnPortHit(this, beam);
    }

    // 레이저 입력 시 호출
    public void EmitEffect(Color laserColor)
    {
        var main = portEffect.main;
        main.startColor = laserColor;     // 레이저 색상 적용

        portEffect.Stop();                // 이전 재생 중지
        portEffect.Play();                // 재생
    }
    public void StopEffect()
    {
        portEffect.Stop();
    }

    /// <summary>
    /// 이 포트의 월드 좌표계에서의 배출 방향 벡터 (파이프 오브젝트의 회전에 따라 계산).
    /// </summary>
    public Vector3 WorldExitDirection
    {
        get
        {
            // exitLocalDirection은 로컬 공간 기준 설정, 이를 월드 공간 방향으로 변환
            return transform.TransformDirection(exitLocalDirection.normalized);
        }
    }
}
