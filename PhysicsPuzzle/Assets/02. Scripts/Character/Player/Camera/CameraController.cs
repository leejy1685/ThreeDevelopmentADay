using System;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Utils;
using Cinemachine;
using UnityEngine;

namespace _02._Scripts.Character.Player.Camera
{
    public class CameraController : MonoBehaviour
    {
        // Components
        [Header("Components")]
        [SerializeField] private PlayerCondition playerCondition;
        
        // Camera Attributes
        [Header("Camera Settings")] 
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float cameraSensitivity;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float cameraVerticalMovement;
        [SerializeField] private float originalCameraPivotAngleX;
        [SerializeField] private Vector2 mouseDelta;

        // Fields
        private CharacterManager _characterManager;
        
        // Properties
        public Transform CameraPivot => cameraPivot;

        private void Start()
        {
            _characterManager = CharacterManager.Instance;
            
            originalCameraPivotAngleX = cameraPivot.localEulerAngles.x;
            playerCondition = _characterManager.Player.PlayerCondition;
        }

        private void LateUpdate()
        {
            if (playerCondition.IsPlayerCharacterHasControl) RotateCamera();
        }

        private void RotateCamera()
        {
            cameraVerticalMovement += mouseDelta.y * cameraSensitivity;
            cameraVerticalMovement = Mathf.Clamp(cameraVerticalMovement, minX, maxX);
            cameraPivot.localEulerAngles = new Vector3(-cameraVerticalMovement + originalCameraPivotAngleX, 0, 0);
            transform.eulerAngles += new Vector3(0, playerCondition.IsPlayerUpsideDown ? -mouseDelta.x * cameraSensitivity : mouseDelta.x * cameraSensitivity, 0);
        }

        #region Player Input Methods

        /// <summary>
        /// Mouse delta will be changed in PlayerInput by invoking this method
        /// </summary>
        /// <param name="delta"></param>
        public void OnLook(Vector2 delta)
        {
            mouseDelta = delta;
        }

        #endregion
    }
}