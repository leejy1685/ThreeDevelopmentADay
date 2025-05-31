using _02._Scripts.Objects.LaserMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDoors : MonoBehaviour
{
    [Header(" [ Linked GoalMachines ] ")]
    [SerializeField] private List<GoalMachine> _linkedGoals = new();

    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;


    [Header(" [ Open Settings ] ")]
    [SerializeField] private float _openDuration = 2f; // 애니메이션 시간
    [SerializeField] private float _openDelay = 2f; // 골 조건 체크 지연 시간
    private bool _isOpen;

    private Coroutine _waitToOpen_Coroutine;

    private void Awake()
    {
        _isOpen = false;
    }

    void Start()
    {
        if (_linkedGoals == null || _linkedGoals.Count == 0)
            Debug.LogWarning("에디터에서 Goal Machine 할당하지 않음!");
    }

    void Update()
    {
        if (_isOpen)
            return;

        if (AllGoalsActivated())
        {
            if (_waitToOpen_Coroutine == null)
                _waitToOpen_Coroutine = StartCoroutine(WaitAndOpen_Coroutine());
        }
        else
        {
            // 조건 도중 해제되면 코루틴 취소
            if (_waitToOpen_Coroutine != null)
            {
                StopCoroutine(_waitToOpen_Coroutine);
                _waitToOpen_Coroutine = null;
            }
        }
    }

    private bool AllGoalsActivated()
    {
        foreach (GoalMachine goal in _linkedGoals)
        {
            if (!goal.IsActivate)
                return false;
        }
        return true;
    }

    private IEnumerator WaitAndOpen_Coroutine()
    {
        float timer = 0f;

        while (timer < _openDelay)
        {
            if (!AllGoalsActivated())
            {
                yield break; // 조건 깨지면 중단
            }

            timer += Time.deltaTime;
            yield return null;
        }

        _isOpen = true;
        StartCoroutine(OpenDoor_Coroutine());
    }

    private IEnumerator OpenDoor_Coroutine()
    {
        Vector3 leftStart = _leftDoor.localPosition;
        Vector3 leftTarget = new Vector3(leftStart.x, leftStart.y, 2f);

        Vector3 rightStart = _rightDoor.localPosition;
        Vector3 rightTarget = new Vector3(rightStart.x, rightStart.y, -3.3f);

        float elapsed = 0f;

        while (elapsed < _openDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _openDuration;

            _leftDoor.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
            _rightDoor.localPosition = Vector3.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        _leftDoor.localPosition = leftTarget;
        _rightDoor.localPosition = rightTarget;
    }
    
}
