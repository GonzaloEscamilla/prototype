using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core
{
    /// <summary>
    /// For single use Interactables
    /// </summary>
    public class SinglePressInteractable : BaseInteractable
    {
        [SerializeField] private bool enableOnAwake;
        [SerializeField] private bool singleUse;
        [SerializeField] private float cooldown;
        
        public UnityEvent OnInteract;

        public bool IsEnable { get; set; }

        private int _usesAmount;
        private bool _isCoolingDown;
        
        private void Start()
        {
            if (!enableOnAwake)
                return;

            IsEnable = true;
        }

        public void Reset()
        {
            StopAllCoroutines();

            _usesAmount = 0;
        }

        public override bool Interact(GameObject interactionSource, out object interactionResultData)
        {
            interactionResultData = null;
            
            if (!IsEnable || _isCoolingDown)
                return false;

            if (singleUse && _usesAmount > 0)
                return false;

            _usesAmount++;

            OnInteract?.Invoke();
            StartCoroutine(Cooldown());
            Debug.Log($"Interacted by {interactionSource}", interactionSource);
            
            return true;
        }
        private IEnumerator Cooldown()
        {
            _isCoolingDown = true;
            yield return new WaitForSeconds(cooldown);
            _isCoolingDown = false;
        }
    }
}