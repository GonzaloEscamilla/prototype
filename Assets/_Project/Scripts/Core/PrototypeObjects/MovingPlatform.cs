using System;
using DG.Tweening;
using KinematicCharacterController;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMoverController
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform initialPositionPivot;
    [SerializeField] private Transform finalPositionPivot;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float waitToMove;
    [SerializeField] private Ease easeType;
    
    private bool _isMoving;
    private bool _isInitialPosition;

    [SerializeField] private Vector3 _myGoalPosition;
    public PhysicsMover Mover;

    private void Awake()
    {
        platform.position = initialPositionPivot.position;
        _isInitialPosition = true;
        _myGoalPosition = transform.position;
        Mover.MoverController = this;
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
            DOTween.To(x => _myGoalPosition.x = x,
                    initialPositionPivot.position.x,
                    finalPositionPivot.position.x,
                    movementSpeed)
                .SetSpeedBased()
                .SetEase(easeType)
                .OnComplete(() => OnMovementCompleted(false))
                .SetDelay(waitToMove);

            DOTween.To(y => _myGoalPosition.y = y,
                    initialPositionPivot.position.y,
                    finalPositionPivot.position.y,
                    movementSpeed)
                .SetSpeedBased()
                .SetEase(easeType)
                .SetDelay(waitToMove);
            
            DOTween.To(z => _myGoalPosition.z = z,
                    initialPositionPivot.position.z,
                    finalPositionPivot.position.z,
                    movementSpeed)
                .SetSpeedBased()
                .SetEase(easeType)
                .SetDelay(waitToMove);
            
            return;
        }
        
        DOTween.To(x => _myGoalPosition.x = x,
                finalPositionPivot.position.x,
                initialPositionPivot.position.x,
                movementSpeed)
            .SetSpeedBased()
            .SetEase(easeType)
            .OnComplete(() => OnMovementCompleted(true))
            .SetDelay(waitToMove);

        DOTween.To(y => _myGoalPosition.y = y,
                finalPositionPivot.position.y,
                initialPositionPivot.position.y,
                movementSpeed)
            .SetSpeedBased()
            .SetEase(easeType)
            .SetDelay(waitToMove);

        DOTween.To(z => _myGoalPosition.z = z,
                finalPositionPivot.position.z,
                initialPositionPivot.position.z,
                movementSpeed)
            .SetSpeedBased()
            .SetEase(easeType)
            .SetDelay(waitToMove);
    }

    private void OnMovementCompleted(bool isInitialPosition)
    {
        _isMoving = false;
        _isInitialPosition = isInitialPosition;
        Move();
    }
    
    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        // Set our platform's goal pose to the animation's
        goalPosition = _myGoalPosition;
        goalRotation = Quaternion.identity;
    }
}
