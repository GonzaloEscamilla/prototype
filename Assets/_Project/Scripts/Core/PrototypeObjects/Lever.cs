using System;
using _Project.Scripts.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private SinglePressInteractable interactable;
    [SerializeField] private float movementDuration;
    [SerializeField] private Ease tweenType;
    [SerializeField] private Transform leverGraphic;
    [SerializeField] private Transform initialPivot;
    [SerializeField] private Transform finalPivot;

    public UnityEvent<bool> OnSwitch;
    
    private bool _isOn;
    private bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            SetLeverGraphics(_isOn);
        }
    }

    private void Awake()
    {
        interactable.OnInteract.AddListener(SwitchLeverState);
        leverGraphic.DOMove(initialPivot.position, 0);
        leverGraphic.DORotateQuaternion(initialPivot.transform.rotation, 0);
    }

    private void OnDestroy()
    {
        interactable.OnInteract.RemoveListener(SwitchLeverState);
    }

    private void SwitchLeverState()
    {
        IsOn = !IsOn;
        OnSwitch.Invoke(IsOn);
    }
    
    private void SetLeverGraphics(bool isOn)
    {
        if (isOn)
        {
            leverGraphic.DOMove(finalPivot.position, movementDuration).SetEase(tweenType);
            leverGraphic.DORotateQuaternion(finalPivot.transform.rotation, movementDuration).SetEase(tweenType);
            return;
        }
        leverGraphic.DOMove(initialPivot.position, movementDuration).SetEase(tweenType);
        leverGraphic.DORotateQuaternion(initialPivot.transform.rotation, movementDuration).SetEase(tweenType);
    }
}
