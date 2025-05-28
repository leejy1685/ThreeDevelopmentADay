using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

/// <summary>
/// 레이저 광선의 방향과 색상 정보를 담는 구조체
/// </summary>
public struct LaserBeam
{
    /// <summary>레이저의 방향 (정규화 벡터)</summary>
    public Vector3 direction;
    /// <summary>레이저의 색상 정보 (열거형)</summary>
    public LASER_COLOR colorType;

    public LaserBeam(Vector3 dir, LASER_COLOR color)
    {
        direction = dir.normalized;
        colorType = color;
    }

    /// <summary>
    /// LaserBeam의 colorType을 Unity Engine의 Color로 변환합니다.
    /// </summary>
    public Color ColorValue
    {
        get
        {
            switch (colorType)
            {
                case LASER_COLOR.Red: return Color.red;
                case LASER_COLOR.Blue: return Color.blue;
                case LASER_COLOR.Green: return Color.green;
                case LASER_COLOR.Purple: return Color.magenta;  // 보라색 -> Magenta 사용
                case LASER_COLOR.Yellow: return Color.yellow;
                case LASER_COLOR.White:
                default: return Color.white;
            }
        }
    }
}

/// <summary>
/// 레이저 색상 종류를 정의한 열거형, LaserMachine에 있어서 주석 처리
/// </summary>
//public enum LAZER_COLOR
//{
//    White,
//    Blue,
//    Green,
//    Purple,
//    Red,
//    Yellow
//}
