using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core
{
    public class HoldInteractable : BaseInteractable
    { 
        [SerializeField] private bool enableOnAwake;
        [SerializeField] private bool singleUse;
        [SerializeField] private float secondsToFill;
        [ShowInInspector] private float fillPercentage;
        
        public UnityEvent OnCompleted;
        public UnityEvent<float> InteractionPerformed;
        
        private float _currentFillAmount;
        private float CurrentFillAmount
        {
            get => _currentFillAmount;
            set
            {
                _currentFillAmount = value;
                
                if (_currentFillAmount <= 0)
                {
                    _currentFillAmount = 0;
                }

                if (_currentFillAmount > secondsToFill)
                {
                    _currentFillAmount = secondsToFill;
                }
            }
        }
        
        public bool IsInteractable { get; set; }
        
        private int usesAmount;
        private bool _completed;
        
        private void Awake()
        {
            if (!enableOnAwake)
                return;

            IsInteractable = true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            _completed = false;
            CurrentFillAmount = 0;
            usesAmount = 0;
        }

        public override void Interact(GameObject interactionSource)
        {
            if (!IsInteractable || _completed)
                return;

            if (singleUse && usesAmount > 0)
                return;

            CurrentFillAmount += Time.deltaTime;

            if (Math.Abs(CurrentFillAmount - secondsToFill) < 0.05f)
            {
                OnCompleted?.Invoke();
                usesAmount++;
                _completed = true;
            }

            fillPercentage = CurrentFillAmount / secondsToFill;
            
            InteractionPerformed?.Invoke(fillPercentage);
            Debug.Log($"Interacted by {interactionSource}, Percentage: {fillPercentage}", interactionSource);
        }
    }
}