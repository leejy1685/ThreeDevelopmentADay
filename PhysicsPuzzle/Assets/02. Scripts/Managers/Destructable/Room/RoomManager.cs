using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using _02._Scripts.Managers.Destructable.Stage;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable.Room
{
    [Serializable] public class RoomData
    {
        public int roomId;
        public List<GoalMachine> goals;
        public Transform leftDoor, rightDoor;
    }
    
    public class RoomManager : MonoBehaviour
    {
        [Header("Room Settings")]
        [SerializeField] private RoomData roomData;

        [Header("Door Settings")] 
        [SerializeField] private float openDelay = 2f;
        [SerializeField] private float openDuration = 2f; // 애니메이션 시간
        
        private bool _isRoomCleared;
        private StageManager _stageManager;
        private Coroutine _waitToOpenCoroutine;
        private float timer = 0f;
        
        public RoomData RoomData => roomData;
        
        [SerializeField] private AudioClip _doorOpenClip;
        
        private void Start()
        {
            _stageManager = StageManager.Instance;
        }

        private void Update()
        {
            CheckGoalsInRoom();
        }

        private void CheckGoalsInRoom()
        {
            if (_isRoomCleared) return;
            if (roomData.goals.Any(goal => !goal.IsActivate))
            {
                timer = 0f;
                return;
            }
            timer += Time.deltaTime;
            if (timer > openDelay)
                OpenDoor();
        }
        
        public void OpenDoor()
        {
            _isRoomCleared = true;
            _stageManager.RoomCleared(roomData.roomId);
            StartCoroutine(OpenDoor_coroutine());
        }

        private IEnumerator OpenDoor_coroutine()
        {
            if(!roomData.leftDoor || !roomData.rightDoor) yield break;
            
            SoundManager.PlaySfx(_doorOpenClip);
            var leftStart = roomData.leftDoor.localPosition;
            var leftTarget = new Vector3(leftStart.x, leftStart.y, 2f);

            var rightStart = roomData.rightDoor.localPosition;
            var rightTarget = new Vector3(rightStart.x, rightStart.y, -3.3f);

            var elapsed = 0f;

            while (elapsed < openDuration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / openDuration;

                roomData.leftDoor.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
                roomData.rightDoor.localPosition = Vector3.Lerp(rightStart, rightTarget, t);

                yield return null;
            }

            roomData.leftDoor.localPosition = leftTarget;
            roomData.rightDoor.localPosition = rightTarget;
            _waitToOpenCoroutine = null;
        }
    }
}