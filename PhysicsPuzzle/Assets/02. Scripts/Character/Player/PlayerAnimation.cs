using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        // Animation Key Const Fields
        private static readonly int Jump = Animator.StringToHash("Jump_trig");
        private static readonly int Speed = Animator.StringToHash("Speed_f");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded_b");
        private static readonly int IsCrouch = Animator.StringToHash("IsCrouch_b");
        
        // Components
        [Header("Components")]
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (!animator) Helper.GetComponentInChildren_Helper<Animator>(gameObject);
        }

        /// <summary>
        /// Set Player Movement Animation (Idle, Walk, Run)
        /// </summary>
        /// <param name="speed"></param>
        public void SetPlayerSpeed(float speed)
        {
            animator.SetFloat(Speed, speed);
        }
        
        /// <summary>
        /// Set Player Ground state (Grounded, Airborne)
        /// </summary>
        /// <param name="isGrounded"></param>
        public void SetPlayerIsGrounded(bool isGrounded)
        {
            animator.SetBool(IsGrounded, isGrounded);
        }

        /// <summary>
        /// Set Player Crouch state (Crouch, Stand)
        /// </summary>
        /// <param name="isCrouch"></param>
        public void SetPlayerIsCrouch(bool isCrouch)
        {
            animator.SetBool(IsCrouch, isCrouch);
            
        }
        
        /// <summary>
        /// Set Player Jump (Triggers jump motion)
        /// </summary>
        public void SetPlayerJump()
        {
            animator.ResetTrigger(Jump);
            animator.SetTrigger(Jump);
        }
    }
}