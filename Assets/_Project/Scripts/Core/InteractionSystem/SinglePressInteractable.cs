using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core
{
    /// <summary>
    /// For single use Interactables
    /// </summary>
    public class SinglePressInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool enableOnAwake;
        [SerializeField] private bool singleUse;
        [SerializeField] private float checkRate;
        [SerializeField] private float detectionRange;
        [SerializeField] private LayerMask detectionLayerMask;
        [SerializeField] private bool lockRotation;
        
        public UnityEvent OnInteract;

        public bool IsEnable { get; set; }

        private int usesAmount;

        private void Start()
        {
            if (!enableOnAwake)
                return;

            IsEnable = true;
        }

        public void Reset()
        {
            StopAllCoroutines();

            usesAmount = 0;
        }

        public void Interact(GameObject interactionSource)
        {
            if (!IsEnable)
                return;

            if (singleUse && usesAmount > 0)
                return;

            OnInteract?.Invoke();
            usesAmount++;
            Debug.Log($"Interacted by {interactionSource}", interactionSource);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}