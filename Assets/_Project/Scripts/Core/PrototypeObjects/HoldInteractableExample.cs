using System;
using _Project.Scripts.Core;
using UnityEngine;

public class HoldInteractableExample : MonoBehaviour
{
   [SerializeField] private HoldInteractable holdInteractable;
   [SerializeField] private Transform platform;
   [SerializeField] private Transform initialPositionPivot;
   [SerializeField] private Transform finalPositionPivot;
   [SerializeField] private Transform rotator;
   
   private void Awake()
   {
      holdInteractable.InteractionPerformed.AddListener(OnInteractionPerformed);
   }

   private void OnInteractionPerformed(float value)
   {
      rotator.transform.Rotate(Vector3.up, 1f);
      platform.position = Vector3.Lerp(initialPositionPivot.position, finalPositionPivot.position, value);
   }
}
