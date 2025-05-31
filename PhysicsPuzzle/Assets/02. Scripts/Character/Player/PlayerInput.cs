using _02._Scripts.Character.Player.Camera;
using _02._Scripts.Managers;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.PIpe.ConnectionPipe;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02._Scripts.Character.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private PlayerCondition playerCondition;

        private CharacterManager _characterManager;
        private InventoryManager _inventoryManager;

        private void Start()
        {
            _characterManager = CharacterManager.Instance;
            _inventoryManager = InventoryManager.Instance;
            
            playerController = _characterManager.Player.PlayerController;
            cameraController = _characterManager.Player.CameraController;
            playerInteraction = _characterManager.Player.PlayerInteraction;
            playerCondition = _characterManager.Player.PlayerCondition;
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

        public void OnChangeGravity(InputAction.CallbackContext context)
        {
            if (!playerCondition.IsGravityAllowed) return;
            if(context.started) playerController.OnChangeGravity();
        }

        public void OnChangeTime(InputAction.CallbackContext context)
        {
            if(context.started) playerController.OnChangeTime();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.started) playerInteraction.OnInteract();
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if(context.started) InventoryManager.Instance.UseItem();
        }
        
        public void OnDrop(InputAction.CallbackContext context)
        {
            if(context.performed) InventoryManager.Instance.DropItem();
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (playerCondition.IsPlayerCharacterHasControl) return;
                switch (playerInteraction.Interactable)
                {
                    case LaserMachine laserMachine:
                        laserMachine.ToggleLaser();
                        break;
                    case ReactiveMachine reactiveMachine:
                        reactiveMachine.ToggleLaser();
                        break;
                }
            }
        }

        public void OnSelectItem(InputAction.CallbackContext context)
        {
            if (!context.performed || !playerCondition.IsPlayerCharacterHasControl) return;
            var val = context.ReadValue<float>();
            Debug.Log(val);
            if(val > 0) _inventoryManager.SelectPrevItem();
            else _inventoryManager.SelectNextItem();
        }

        public void OnGodMode(InputAction.CallbackContext context)
        {
            if(context.started) playerCondition.ToggleGodMode();
        }
        
        #endregion
    }
}