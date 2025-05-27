using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02._Scripts.Character.Player
{
    public class PlayerCondition : MonoBehaviour
    {
        [Header("Player Character Condition")]
        [SerializeField] private bool isPlayerCharacterHasControl = true;
        
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;
        
        public bool IsPlayerCharacterHasControl => isPlayerCharacterHasControl;
        public bool IsPlayerFiring { get; private set; }

        public void MigrateCameraFocusToOtherObject(Transform other)
        {
            if (isPlayerCharacterHasControl)
            {
                thirdPersonCamera.Follow = other;
                thirdPersonCamera.LookAt = other;
                firstPersonCamera.Priority = 0;
                thirdPersonCamera.Priority = 10;
            }
            else
            {
                firstPersonCamera.Priority = 10;
                thirdPersonCamera.Priority = 0;
            }
                
            isPlayerCharacterHasControl = !isPlayerCharacterHasControl;
        }
    }
}