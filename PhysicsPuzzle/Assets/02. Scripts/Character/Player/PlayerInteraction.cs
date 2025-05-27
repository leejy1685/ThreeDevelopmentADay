using System;
using _02._Scripts.Utils.Interface;
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
        
        // Properties
        public IInteractable Interactable { get; private set; }

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
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
            Interactable.OnInteract();
            interactableObject = null;
            Interactable = null;
        }
    }
}