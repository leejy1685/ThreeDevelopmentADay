using System;
using _02._Scripts.Managers;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils.Interface;
using JetBrains.Annotations;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")] 
        [SerializeField] private float checkRate = 0.05f;
        [SerializeField] private float maxCheckDistance = 5f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private GameObject interactableObject;
        
        // Fields
        private float _timeSinceLastCheck;
        private UnityEngine.Camera _camera;
        private bool _isPlayerHoldingProp;
        private PlayerCondition _playerCondition;
        
        // Properties
        public IInteractable Interactable { get; private set; }

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
            _playerCondition = CharacterManager.Instance.Player.PlayerCondition;
        }

        private void Update()
        {
            if (!_playerCondition.IsPlayerCharacterHasControl) return; 
            if (_timeSinceLastCheck < checkRate) { _timeSinceLastCheck += Time.deltaTime; return; }
            
            _timeSinceLastCheck = 0f;
            var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out var hit, maxCheckDistance, interactableLayer))
            {
                if (hit.collider.gameObject == interactableObject) return;
                
                interactableObject = hit.collider.gameObject;
                if (!interactableObject.TryGetComponent<IInteractable>(out var interactable)) return;
                Interactable = interactable;
            }
            else
            {
                interactableObject = null;
                Interactable = null;
            }
        }

        public void OnInteract()
        {
            if (Interactable == null) return;
            
            Interactable?.OnInteract();
            
            if (Interactable is LaserMachine) return;
            interactableObject = null;
            Interactable = null;
        }
    }
}