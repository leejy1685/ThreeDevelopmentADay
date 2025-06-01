using System.Collections;
using _02._Scripts.Character.Player.Interface;
using UnityEngine;

namespace _02._Scripts.PIpe.ConnectionPipe
{
    public class LinkedPipeRotation : MonoBehaviour, IInteractable
    {
        [Header("Pipe Rotation Pivot")] 
        [SerializeField] private float rotationAngle = 90f;
        [SerializeField] private float rotationDuration = 0.5f;
        [SerializeField] private Transform rotationPivot;
        
        private Coroutine _rotationRoutine;
        
        public void OnInteract()
        {
            _rotationRoutine ??= StartCoroutine(RotateAroundPivot());
        }
    
        private IEnumerator RotateAroundPivot()
        {
            var startRotation = transform.rotation;
            var targetRotation = startRotation * Quaternion.Euler(Vector3.up * rotationAngle);

            var elapsed = 0f;

            while (elapsed < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / rotationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            _rotationRoutine = null;
        }
    }
}