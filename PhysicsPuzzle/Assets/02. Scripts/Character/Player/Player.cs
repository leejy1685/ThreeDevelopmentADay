using _02._Scripts.Character.Player.Camera;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    [RequireComponent(typeof(PlayerController), typeof(PlayerAnimation), typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerCondition), typeof(CameraController), typeof(PlayerEquipment))]
    [RequireComponent(typeof(PlayerInteraction))]
    public class Player : MonoBehaviour
    {
        [Header("Necessary Components")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerCondition playerCondition;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerEquipment playerEquipment;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private Transform itemThrowPivot;
        [SerializeField] private Transform equipmentPivot;
        
        // Properties
        public PlayerController PlayerController => playerController;
        public PlayerAnimation PlayerAnimation => playerAnimation;
        public PlayerInput PlayerInput => playerInput;
        public PlayerCondition PlayerCondition => playerCondition;
        public PlayerInteraction PlayerInteraction => playerInteraction;
        public PlayerEquipment PlayerEquipment => playerEquipment;
        public CameraController CameraController => cameraController;
        public Transform ItemThrowPivot => itemThrowPivot;
        public Transform EquipmentPivot => equipmentPivot;
        
        private void Awake()
        {
            if (!playerController) playerController = Helper.GetComponent_Helper<PlayerController>(gameObject);
            if (!playerAnimation) playerAnimation = Helper.GetComponent_Helper<PlayerAnimation>(gameObject);
            if (!playerInput) playerInput = Helper.GetComponent_Helper<PlayerInput>(gameObject);
            if (!playerCondition) playerCondition = Helper.GetComponent_Helper<PlayerCondition>(gameObject);
            if (!cameraController) cameraController = Helper.GetComponent_Helper<CameraController>(gameObject);
            if (!playerEquipment) playerEquipment = Helper.GetComponent_Helper<PlayerEquipment>(gameObject);
            if (!playerInteraction) playerInteraction = Helper.GetComponent_Helper<PlayerInteraction>(gameObject);
        }
    }
}