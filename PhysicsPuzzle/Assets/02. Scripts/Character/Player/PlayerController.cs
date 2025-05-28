using System;
using _02._Scripts.Character.Player.Camera;
using _02._Scripts.Managers;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private PlayerCondition playerCondition;
        [SerializeField] private PlayerAnimation playerAnimator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerInteraction playerInteraction;
        
        [Header("Movement Settings")]
        [SerializeField] private Vector2 movementDirection;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float crouchSpeed = 3f;
        [SerializeField] private float speedDeltaMultiplier = 1f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravityValue = 9.81f;
        [SerializeField] private Vector3 gravityDirection = Vector3.down;
        [SerializeField] private LayerMask groundLayer;

        [Header("CameraPivot Settings")] 
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float crouchCameraPositionY = 1f;
        [SerializeField] private float originalCameraPositionY;
        
        // Components
        private Transform _cameraPivot;
        private CharacterManager _characterManager;
        
        // Player State Fields
        private bool _isGrounded = true;
        private bool _isCrouch;
        
        // Player Input Checking Fields
        private bool _isJumpPressed;
        private bool _isCrouchPressed;
        private bool _isPlayerUpsideDown;
        
        // Properties
        public bool IsPlayerUpsideDown => _isPlayerUpsideDown;

        private void Awake()
        {
            if (!rigidBody) rigidBody = Helper.GetComponent_Helper<Rigidbody>(gameObject);
            if (!capsuleCollider) capsuleCollider = Helper.GetComponent_Helper<CapsuleCollider>(gameObject);
            if (!cameraPivot) { Debug.LogError("Missing Component : CameraPivot is Missing!"); throw new MissingComponentException(); }
        }

        private void Start()
        {
            _cameraPivot = cameraController.CameraPivot;
            _characterManager = CharacterManager.Instance;
            originalCameraPositionY = _cameraPivot.localPosition.y;
            
            playerAnimator = _characterManager.Player.PlayerAnimation;
            cameraController = _characterManager.Player.CameraController;
            playerCondition = _characterManager.Player.PlayerCondition;
            playerInteraction = _characterManager.Player.PlayerInteraction;
        }

        private void FixedUpdate()
        {
            if (!playerCondition.IsPlayerCharacterHasControl) return;
            CalculateMovement();
        }

        private void Update()
        {
            if (!playerCondition.IsPlayerCharacterHasControl)
            {
                if(playerInteraction.Interactable is LaserMachine laserMachine)
                    laserMachine.ControlLaserPitch(movementDirection);
            }
        }

        private void LateUpdate()
        {
            if (!playerCondition.IsPlayerCharacterHasControl) return;
            SetPositionOfCameraPivot();
        }

        /// <summary>
        /// Calculate Player Movement
        /// </summary>
        private void CalculateMovement()
        {
            _isGrounded = IsPlayerGrounded(); 
            playerAnimator.SetPlayerIsGrounded(_isGrounded);
            
            var move = CalculateMovement_FlatSurface();
            
            if (_isGrounded)
            {
                if(gravityDirection == Vector3.down)
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, -1f, rigidBody.velocity.z);
                else if(gravityDirection == Vector3.up)
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 1f, rigidBody.velocity.z);
            }else { rigidBody.velocity += gravityDirection * (gravityValue * rigidBody.mass * Time.fixedDeltaTime); }
            
            if (_isGrounded && _isJumpPressed)
            {
                playerAnimator.SetPlayerJump();
                rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                
                _isJumpPressed = false;
            } 
            
            rigidBody.MovePosition(transform.position + move);
        }

        /// <summary>
        /// Calculate Player Movement in flat surface.
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateMovement_FlatSurface()
        {
            var targetSpeed = 0f;
            if (movementDirection != Vector2.zero) { targetSpeed = _isCrouchPressed ? crouchSpeed : maxSpeed; }
            if (currentSpeed <= 0.1f) currentSpeed = !Mathf.Approximately(currentSpeed, targetSpeed) ? Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * speedDeltaMultiplier) : targetSpeed;
            else currentSpeed = !Mathf.Approximately(currentSpeed, targetSpeed) ? Mathf.Lerp(currentSpeed, targetSpeed, currentSpeed / targetSpeed * speedDeltaMultiplier * Time.fixedDeltaTime) : targetSpeed;
            
            var velocityXZ = (transform.forward * movementDirection.y + transform.right * movementDirection.x).normalized * currentSpeed;
            playerAnimator.SetPlayerSpeed(velocityXZ.magnitude / maxSpeed);
            return velocityXZ * Time.fixedDeltaTime;
        }

        /// <summary>
        /// Set Position of CameraPivot
        /// </summary>
        private void SetPositionOfCameraPivot()
        {
            if (!_isCrouch) return;
            if (_isCrouchPressed) {
                if (!Mathf.Approximately(cameraPivot.localPosition.y, crouchCameraPositionY))
                {
                    var crouchCameraPosition = cameraPivot.localPosition;
                    crouchCameraPosition.y = crouchCameraPositionY;
                    cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, crouchCameraPosition,
                        (crouchCameraPosition.y / cameraPivot.localPosition.y * speedDeltaMultiplier) * Time.deltaTime);
                }
                else
                {
                    var crouchCameraPosition = cameraPivot.localPosition;
                    crouchCameraPosition.y = crouchCameraPositionY;
                    cameraPivot.localPosition = crouchCameraPosition;
                    _isCrouch = false;
                }
            }
            else
            {
                if (!Mathf.Approximately(cameraPivot.localPosition.y, originalCameraPositionY))
                {
                    var originalCameraPosition = cameraPivot.localPosition;
                    originalCameraPosition.y = originalCameraPositionY;
                    cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, originalCameraPosition,
                        (originalCameraPositionY / cameraPivot.localPosition.y * speedDeltaMultiplier) * Time.deltaTime);
                }
                else
                {
                    var originalCameraPosition = cameraPivot.localPosition;
                    originalCameraPosition.y = originalCameraPositionY;
                    cameraPivot.localPosition = originalCameraPosition;
                    _isCrouch = false;
                }
            }
        }

        /// <summary>
        /// Check if the player is on the Ground.
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerGrounded()
        {
            return Physics.CheckSphere(transform.position + (transform.up * 0.25f), 0.35f, groundLayer);
        }
        
        #region Player Input Methods

        /// <summary>
        /// Set Movement Direction.
        /// </summary>
        /// <param name="direction"></param>
        public void OnMove(Vector2 direction)
        {
            movementDirection = direction;
        }

        /// <summary>
        /// Set Jump to true if the player is on the ground.
        /// </summary>
        /// <param name="jump"></param>
        public void OnJump(bool jump)
        {
            if(_isGrounded && playerCondition.IsPlayerCharacterHasControl) _isJumpPressed = jump;
        }

        /// <summary>
        /// Set crouch to true and play crouch animation.
        /// </summary>
        public void OnCrouch()
        {
            if (!playerCondition.IsPlayerCharacterHasControl) return;
            _isCrouchPressed = !_isCrouchPressed;
            _isCrouch = true;
            playerAnimator.SetPlayerIsCrouch(_isCrouchPressed);
            capsuleCollider.center = _isCrouchPressed ? capsuleCollider.center / 2 : capsuleCollider.center * 2;
            capsuleCollider.height = _isCrouchPressed ? 1 : 2;
        }

        /// <summary>
        /// Change Gravity Direction
        /// </summary>
        public void OnChangeGravity()
        {
            if (!playerCondition.IsPlayerCharacterHasControl) return;
            if (!playerCondition.IsGravitySkillAvailable) return;
            
            if (gravityDirection == Vector3.down)
            {
                gravityDirection = Vector3.up;
                transform.position += transform.up * capsuleCollider.height; 
                transform.rotation = Quaternion.Euler(180, 0, 0);
            }
            else
            {
                gravityDirection = Vector3.down;
                transform.position += transform.up * capsuleCollider.height; 
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            _isPlayerUpsideDown = !_isPlayerUpsideDown;
        }
        
        #endregion
    }
}