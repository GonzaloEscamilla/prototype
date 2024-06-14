using _Project.Scripts.Utilities.Math;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public struct MoveInputData
    {
        public Vector2 MoveInputVector;
        public Vector2 LookInputVector;
        
        public MoveInputData(Vector2 moveInputVector, Vector2 lookInputVector)
        {
            MoveInputVector = moveInputVector;
            LookInputVector = lookInputVector;
        }
    }
    
    public class CharacterMovementController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;

        [Header("Misc")]
        public bool RotationObstruction;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Transform MeshRoot;
        public bool OrientTowardsGravity;

        [Sirenix.OdinInspector.ReadOnly] public Vector3 _moveInputVector;
        [Sirenix.OdinInspector.ReadOnly] public Vector3 _lookInputVector;

        private Camera _mainCamera;
        
        private void Awake()
        {
            Motor.CharacterController = this;
            _mainCamera = Camera.main;
        }

        public void SetInputs(MoveInputData moveData)
        {
            _moveInputVector = moveData.MoveInputVector.XZ().normalized;
            _lookInputVector = moveData.LookInputVector.XZ().normalized;
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
            {
                // Adjust the look input vector to be relative to the camera
                Vector3 cameraForward = _mainCamera.transform.forward;
                Vector3 cameraRight = _mainCamera.transform.right;

                // Project the camera forward and right directions onto the horizontal plane
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward.Normalize();
                cameraRight.Normalize();

                Vector3 adjustedLookInput = cameraForward * _lookInputVector.z + cameraRight * _lookInputVector.x;

                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, adjustedLookInput, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }

            if (OrientTowardsGravity)
            {
                // Rotate from current up to invert gravity
                currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), -Gravity) * currentRotation;
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;

            // Adjust the move input vector to be relative to the camera
            Vector3 cameraForward = _mainCamera.transform.forward;
            Vector3 cameraRight = _mainCamera.transform.right;

            // Project the camera forward and right directions onto the horizontal plane
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 adjustedMoveInput = cameraForward * _moveInputVector.z + cameraRight * _moveInputVector.x;

            if (Motor.GroundingStatus.IsStableOnGround)
            {
                targetMovementVelocity = adjustedMoveInput * MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (adjustedMoveInput.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = adjustedMoveInput * MaxAirMoveSpeed;

                    // Prevent climbing on unstable slopes with air movement
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                    currentVelocity += velocityDiff * (AirAccelerationSpeed * deltaTime);
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }
        }


        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}