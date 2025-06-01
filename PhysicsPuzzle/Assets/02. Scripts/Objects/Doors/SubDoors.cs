using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

namespace _02._Scripts.Objects.Doors
{
    public class SubDoors : MonoBehaviour
    {
        [Header(" [ Linked GoalMachines ] ")]
        [SerializeField] private List<GoalMachine> linkedGoals = new();

        [Header(" [ Door Transform ] ")]
        [SerializeField] private Transform leftDoor;
        [SerializeField] private Transform rightDoor;
        
        [Header(" [ Open Settings ] ")]
        [SerializeField] private float openDuration = 2f; // 애니메이션 시간
        [SerializeField] private float openDelay = 2f; // 골 조건 체크 지연 시간
        
        [Header("Door Open Clip")]
        [SerializeField] private AudioClip doorOpenClip;
        
        // Fields
        private bool _isOpen;
        private Coroutine _waitToOpen_Coroutine;

        private void Awake()
        {
            _isOpen = false;
        }

        void Start()
        {
            if (linkedGoals == null || linkedGoals.Count == 0)
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
            foreach (GoalMachine goal in linkedGoals)
            {
                if (!goal.IsActivate)
                    return false;
            }
            return true;
        }

        private IEnumerator WaitAndOpen_Coroutine()
        {
            float timer = 0f;

            while (timer < openDelay)
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
            SoundManager.PlaySfx(doorOpenClip);
        }

        private IEnumerator OpenDoor_Coroutine()
        {
            Vector3 leftStart = leftDoor.localPosition;
            Vector3 leftTarget = new Vector3(leftStart.x, leftStart.y, 2f);

            Vector3 rightStart = rightDoor.localPosition;
            Vector3 rightTarget = new Vector3(rightStart.x, rightStart.y, -3.3f);

            float elapsed = 0f;

            while (elapsed < openDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / openDuration;

                leftDoor.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
                rightDoor.localPosition = Vector3.Lerp(rightStart, rightTarget, t);

                yield return null;
            }

            leftDoor.localPosition = leftTarget;
            rightDoor.localPosition = rightTarget;
        }

    }
}
