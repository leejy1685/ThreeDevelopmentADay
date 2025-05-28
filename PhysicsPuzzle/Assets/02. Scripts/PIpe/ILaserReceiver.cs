using UnityEngine;

/// <summary>
/// 레이저를 수신하는 객체가 구현하는 인터페이스
/// LaserBeam 정보를 받아 처리합니다.
/// </summary>
public interface ILaserReceiver
{
    /// <summary>
    /// 레이저가 충돌했을 때 호출되는 함수. 레이저 정보를 인자로 받습니다.
    /// </summary>
    /// <param name="beam">충돌한 레이저의 정보 (방향, 색상).</param>
    void OnLaserHit(LaserBeam beam);
}
