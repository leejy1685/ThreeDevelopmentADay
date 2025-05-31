using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.UI;
using Cinemachine;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerCondition : MonoBehaviour
    {
        [Header("Player Character Condition")]
        [SerializeField] private bool isPlayerCharacterHasControl = true;
        [SerializeField] private float timeChangeCooldown = 3f;
        [SerializeField] private float gravityChangeCooldown = 3f;
        [SerializeField] private bool isGravityAllowed;
        
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;

        // Fields
        private float _timeSinceLastTimeSkill;
        private float _timeSinceLastGravitySkill;
        private GameUI _gameUI;
        private Player _player;
        private PlayerController _playerController;
        
        // Properties
        public bool IsGravitySkillAvailable { get; private set; } = true;
        public bool IsTimeSkillAvailable { get; private set; } = true;
        public bool IsPlayerCharacterHasControl => isPlayerCharacterHasControl;
        public bool IsPlayerUpsideDown { get; private set; }
        public bool IsGodMode { get; private set; }
        public bool IsMoonTime { get; private set; }
        public bool IsGravityAllowed => isGravityAllowed;

        private void Start()
        {
            _gameUI = UIManager.Instance.GameUI;
            _player = CharacterManager.Instance.Player;
            _playerController = _player.PlayerController;
        }

        public void SetGravityAllow(bool isAllowed)
        {
            isGravityAllowed = isAllowed;
        }

        public void ToggleGodMode()
        {
            IsGodMode = !IsGodMode;
            _playerController.ToggleGodModePhysics();
        }

        public void RunGravitySkillCoolTime()
        {
            _gameUI.ChangeGravity(IsPlayerUpsideDown);
            IsGravitySkillAvailable = false;
            IsPlayerUpsideDown = !IsPlayerUpsideDown;
            
            StartCoroutine(HandleGravitySkillCoolTime());
        }

        public void RunTimeSkillCoolTime()
        {
            IsMoonTime = !IsMoonTime;
            _gameUI.ChangeTime(IsMoonTime);
            IsTimeSkillAvailable = false;
            
            StartCoroutine(HandleTimeSkillCoolTime());
        }

        private IEnumerator HandleGravitySkillCoolTime()
        {
            var currentTime = gravityChangeCooldown;
            _gameUI.ChangeGravityIconFillAmount(1);
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                _gameUI.ChangeGravityIconFillAmount(currentTime / gravityChangeCooldown);
                yield return null;
            }
            _gameUI.ChangeGravityIconFillAmount(0);
            IsGravitySkillAvailable = true;
        }

        private IEnumerator HandleTimeSkillCoolTime()
        {
            var currentTime = timeChangeCooldown;
            _gameUI.ChangeTimeIconFillAmount(1);
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                _gameUI.ChangeTimeIconFillAmount(currentTime / timeChangeCooldown);
                yield return null;
            }
            _gameUI.ChangeTimeIconFillAmount(0);
            IsTimeSkillAvailable = true;
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