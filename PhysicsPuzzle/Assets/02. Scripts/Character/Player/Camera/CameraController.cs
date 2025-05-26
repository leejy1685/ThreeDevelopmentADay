using System;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player.Camera
{
    public class CameraController : MonoBehaviour
    {
        // Components
        [Header("Components")] 
        [SerializeField] private PlayerController playerController;
        
        // Camera Attributes
        [Header("Camera Settings")] 
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float cameraSensitivity;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float cameraVerticalMovement;
        [SerializeField] private Vector2 mouseDelta;

        // Properties
        public Transform CameraPivot => cameraPivot;

        private void Awake()
        {
            if (!playerController) playerController = Helper.GetComponent_Helper<PlayerController>(gameObject);
        }

        private void LateUpdate()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            cameraVerticalMovement += mouseDelta.y * cameraSensitivity;
            cameraVerticalMovement = Mathf.Clamp(cameraVerticalMovement, minX, maxX);
            cameraPivot.localEulerAngles = new Vector3(-cameraVerticalMovement, 0, 0);
            transform.eulerAngles += new Vector3(0, playerController.IsPlayerUpsideDown ? -mouseDelta.x * cameraSensitivity : mouseDelta.x * cameraSensitivity, 0);
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