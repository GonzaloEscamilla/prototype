using System.Collections;
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
        public bool RunInputPressed;
        
        public MoveInputData(Vector2 moveInputVector, Vector2 lookInputVector, bool runInputPressed)
        {
            MoveInputVector = moveInputVector;
            LookInputVector = lookInputVector;
            RunInputPressed = runInputPressed;
        }
    }
    
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class CharacterMovementController : MonoBehaviour, ICharacterController
    {
        [SerializeField] 
        private CharacterWeaponController weaponController;
        
        [SerializeField]
        private CharacterSettings settings;

        
        [ShowInInspector] 
        public bool IsDashing => _isDashing;
    
        private KinematicCharacterMotor _motor;
        private Camera _mainCamera;
        
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private Vector3 _lastVelocity;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _isRunning;
        private bool _isDashBuffered;
        private float _dashElapsedTime = 0;
        private bool _isDashing;

        private void Awake()
        {
            _mainCamera = Camera.main;

            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;

            weaponController.AttackStarted += OnAttackStarted;
        }
        
        public void SetInputs(MoveInputData moveData)
        {
            _moveInputVector = moveData.MoveInputVector.XZ().normalized;
            _lookInputVector = moveData.LookInputVector.XZ().normalized;
            _isRunning = moveData.RunInputPressed;
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_isDashing)
            {
                return;
            }
            
            if (_lookInputVector != Vector3.zero && settings.orientationSharpness > 0f)
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
                Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, adjustedLookInput, 1 - Mathf.Exp(-settings.orientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
            }

            if (settings.orientTowardsGravity)
            {
                // Rotate from current up to invert gravity
                currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), -settings.gravity) * currentRotation;
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
            
            if (_motor.GroundingStatus.IsStableOnGround)
            {
                targetMovementVelocity = GetGroundVelocity(adjustedMoveInput);;
                
                if (_isDashing)
                {
                    targetMovementVelocity = _motor.CharacterForward * settings.maxStableDashMoveSpeed;
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-settings.stableMovementSharpness * deltaTime));
                    return;
                }
                
                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-settings.stableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (adjustedMoveInput.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = adjustedMoveInput * settings.maxAirMoveSpeed;

                    // Prevent climbing on unstable slopes with air movement
                    if (_motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal), _motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, settings.gravity);
                    currentVelocity += velocityDiff * (settings.airAccelerationSpeed * deltaTime);
                }

                // Gravity
                currentVelocity += settings.gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (settings.drag * deltaTime)));
            }
            
            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        private Vector3 GetGroundVelocity(Vector3 adjustedMoveInput)
        {
            var velocity = adjustedMoveInput * (weaponController.IsAttacking ? settings.maxStableMoveSpeedWhenAttacking : settings.maxStableMoveSpeed);
            return velocity;
        }

        public void Dash()
        {
            Debug.LogWarning("Dash");
            if (_isDashing)
            {
                // if (settings.bufferedDashPercentage <= _dashElapsedTime / settings.dashDuration)
                //     _isDashBuffered = true;

                if (settings.bufferedDashPercentage <= _dashElapsedTime / settings.dashDuration)
                {
                    _isDashBuffered = true;
                }
                return;
            }
        
            _isDashing = true;
            StartCoroutine(DashCoroutine());
        }
    
        private IEnumerator DashCoroutine()
        {
            // characterController.detectCollisions = false;

            _dashElapsedTime = 0;
           
            yield return null;
        
            while (_dashElapsedTime <= settings.dashDuration)
            {
                var dashPercentage = _dashElapsedTime / settings.dashDuration;
                // _isInvulnerable = false;
                
                // if (dashPercentage >= settings.dashInvulnerabilityRange.x && dashPercentage <= settings.dashInvulnerabilityRange.y)
                // {
                //     _isInvulnerable = true;
                // }
                
                Debug.LogWarning("Is Dashing");
                _dashElapsedTime += Time.deltaTime;
                yield return null;
            }

            _isDashing = false;
            // characterController.detectCollisions = true;
            // _isInvulnerable = false;

            if (!_isDashBuffered) 
                yield break;
        
            _isDashing = true;
            _isDashBuffered = false;
        
            StartCoroutine(DashCoroutine());
        }
        
        private void OnAttackStarted()
        {
            AddVelocity(_motor.CharacterForward * settings.attackImpulse);
        }
        
        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
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