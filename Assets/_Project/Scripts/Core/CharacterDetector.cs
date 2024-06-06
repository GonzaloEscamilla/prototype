using System;
using _Project.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class CharacterDetector : MonoBehaviour
    {
        [SerializeField] private DetectorSettings detectorSettings;
        [SerializeField] private Canvas debugCanvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Detector<TopDownCharacterController> characterDetector;
        public float fixedXAngle = 0f;

        private void Awake()
        {
            mainCamera = Camera.main;
            canvasGroup.DOFade(0, 0f);

            characterDetector = new Detector<TopDownCharacterController>(transform, detectorSettings);
            characterDetector.OnTargetDetected += CharacterDetected;
            characterDetector.OnTargetLost += CharacterLost;
        }

        private void Update()
        {
            characterDetector.Update();
            
            Vector3 targetPosition = mainCamera.transform.position;
            targetPosition.y = debugCanvas.transform.position.y; // Keep the same y position as the debugCanvas
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - debugCanvas.transform.position);

            // Apply the fixed x angle rotation
            targetRotation *= Quaternion.Euler(fixedXAngle, 0f, 0f);

            // Set the rotation of the debugCanvas
            debugCanvas.transform.rotation = targetRotation;
        }

        private void CharacterDetected(TopDownCharacterController character)
        {
            ShowIndicator();
            Debug.LogWarning("CharacterDetected");
        }
        
        private void CharacterLost(TopDownCharacterController character)
        {
            HideIndicator();
            Debug.LogWarning("Character Lost");
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
            indicatorShown = true;
            canvasGroup.DOFade(1, 0.5f);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, detectorSettings.radius);
        }
    }
}