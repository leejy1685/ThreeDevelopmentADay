using System;
using _02._Scripts.Character.Player.Camera;
using _02._Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02._Scripts.Character.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
            if (!playerController) playerController = Helper.GetComponent_Helper<PlayerController>(gameObject);
            if (!cameraController) cameraController = Helper.GetComponent_Helper<CameraController>(gameObject);
        }
        
        #region Input Actions

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!context.performed) { playerController.OnMove(Vector2.zero); return; }
            playerController.OnMove(context.ReadValue<Vector2>().normalized);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.started) playerController.OnJump(true);
            else if(context.canceled) playerController.OnJump(false);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            cameraController.OnLook(context.ReadValue<Vector2>());
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(context.started) playerController.OnCrouch();
        }
        
        #endregion
    }
}