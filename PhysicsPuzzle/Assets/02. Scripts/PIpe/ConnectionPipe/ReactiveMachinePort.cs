using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.PIpe.ConnectionPipe;
using UnityEngine;

public class ReactiveMachinePort : MonoBehaviour
{
    [SerializeField] private ReactiveMachine _paretMachine;
    private LinkedPipe _currentPipe = null;


    private void Update()
    {
        if (_currentPipe != null)
        {
            var color = _currentPipe.receivedColor;
            _paretMachine.SetLineColor(color);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LinkedPipe"))
        {
            LinkedPipe pipe = other.GetComponentInParent<LinkedPipe>();
            if (pipe != null)
            {
                _currentPipe = pipe;
                // Debug.Log($"{pipe.name} 에서 전달받은 색: {pipe.receivedColor}");
                ReactToColor(pipe.receivedColor);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LinkedPipe"))
        {
            // 현재 연결이 끊어진 파이프라면 초기화
            if (_currentPipe == other.GetComponentInParent<LinkedPipe>())
            {
                _currentPipe = null;
                ReactToColor(LASER_COLOR.White);
            }
        }
    }

    private void ReactToColor(LASER_COLOR color)
    {
        // Debug.Log($"[ReactivePort] 색에 반응함: {color}");
        _paretMachine.SetLineColor(color);
    }
}
