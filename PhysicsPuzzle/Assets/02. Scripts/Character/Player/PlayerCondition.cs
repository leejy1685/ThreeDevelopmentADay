using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02._Scripts.Character.Player
{
    public class PlayerCondition : MonoBehaviour
    {
        [Header("Player Character Condition")]
        [SerializeField] private bool isPlayerCharacterHasControl = true;
        [SerializeField] private float timeChangeCooldown = 3f;
        [SerializeField] private float gravityChangeCooldown = 3f;
        
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;

        // Fields
        private float _timeSinceLastTimeSkill;
        private float _timeSinceLastGravitySkill;
        
        // Properties
        public bool IsGravitySkillAvailable { get; private set; }
        public bool IsTimeSkillAvailable { get; set; }
        public bool IsPlayerCharacterHasControl => isPlayerCharacterHasControl;
        public bool IsPlayerFiring { get; private set; }

        public void RunGravitySkillCoolTime()
        {
            IsGravitySkillAvailable = false;

            StartCoroutine(HandleGravitySkillCoolTime());
        }

        private IEnumerator HandleGravitySkillCoolTime()
        {
            var currentTime = gravityChangeCooldown;
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                yield return null;
            }
            IsGravitySkillAvailable = true;
        }
        
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