using _02._Scripts.Managers.Destructable;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerCharacterIK : MonoBehaviour
    {
        [Header("Necessary Components")]
        [SerializeField] private Player player;
        [SerializeField] private PlayerEquipment playerEquipment;
        [SerializeField] private Animator animator;
        
        private void Awake()
        {
            if (!animator) animator = Helper.GetComponent_Helper<Animator>(gameObject);
        }

        private void Start()
        {
            player = CharacterManager.Instance.Player;
            playerEquipment = player.PlayerEquipment;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (playerEquipment.IsEquipped && player.EquipmentPivot)
            {
                // IK 활성화
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

                // IK 목표 설정
                animator.SetIKPosition(AvatarIKGoal.RightHand, player.EquipmentPivot.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, player.EquipmentPivot.rotation);
            }
            else
            {
                // IK 비활성화
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            }
        }
    }
}