using System;
using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform initialPositionPivot;
    [SerializeField] private Transform finalPositionPivot;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Ease easeType;

    private bool _isMoving;
    private bool _isInitialPosition;
    
    private void Awake()
    {
        platform.position = initialPositionPivot.position;
        _isInitialPosition = true;
    }

    public void Move()
    {
        if (_isMoving)
        {
            return;
        }
        _isMoving = true;

        if (_isInitialPosition)
        {
            platform.DOMove(finalPositionPivot.position, movementSpeed)
                .SetEase(easeType)
                .SetSpeedBased()
                .OnComplete(() => OnMovementCompleted(false));
            return;
        }
        
        platform.DOMove(initialPositionPivot.position, movementSpeed)
            .SetEase(easeType)
            .SetSpeedBased()
            .OnComplete(() => OnMovementCompleted(true));
    }

    private void OnMovementCompleted(bool isInitialPosition)
    {
        _isMoving = false;
        _isInitialPosition = isInitialPosition;
    }
}
