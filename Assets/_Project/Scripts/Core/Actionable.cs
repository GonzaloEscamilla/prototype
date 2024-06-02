using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core
{
    public interface IInteractable
    {
        public void Interact();
    }

    public class Actionable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool oneUse;
        [SerializeField] private float checkRate;
        [SerializeField] private float detectionRange;
        [SerializeField] private LayerMask detectionLayerMask;
        [SerializeField] private bool lockRotation;
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private bool enableOnAwake;

        public UnityEvent OnInteract;

        public bool IsEnable { get; set; }

        private int usesAmount;

        private void Start()
        {
            canvasGroup.alpha = 0;
            StartCoroutine(DetectCoroutine());

            mainCamera = Camera.main;

            if (!enableOnAwake)
                return;

            IsEnable = true;
        }

        public void Reset()
        {
            StopAllCoroutines();

            canvasGroup.alpha = 0;
            StartCoroutine(DetectCoroutine());

            usesAmount = 0;
        }

        private void Update()
        {
            if (!indicatorShown)
                return;

            if (lockRotation)
                return;
            
            canvas.transform.LookAt(mainCamera.transform.position);
        }

        public void Interact()
        {
            if (!IsEnable)
                return;

            if (oneUse && usesAmount > 0)
                return;

            OnInteract?.Invoke();
            HideIndicator();
            usesAmount++;
        }
    
        private IEnumerator DetectCoroutine()
        {
            while (isActiveAndEnabled)
            {
                if (oneUse && usesAmount > 0)
                    yield break;
                
                var collisions = Physics.OverlapSphere(transform.position, detectionRange, detectionLayerMask);

                bool characterDetected = collisions.Any(t => t.GetComponent<TopDownCharacterController>());

                if (characterDetected)
                    ShowIndicator();
                else
                    HideIndicator();

                yield return new WaitForSeconds(checkRate);
            }
        }

        private void HideIndicator()
        {
            if (!indicatorShown)
                return;

            indicatorShown = false;
            canvasGroup.DOFade(0, 0.5f);
        }

        private bool indicatorShown;
        
        private void ShowIndicator()
        {
            if (indicatorShown || !IsEnable)
                return;

            indicatorShown = true;
            canvasGroup.DOFade(1, 0.5f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}