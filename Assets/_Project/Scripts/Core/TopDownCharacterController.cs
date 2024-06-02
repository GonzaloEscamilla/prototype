using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core
{
    public class TopDownCharacterController : MonoBehaviour
    {
        [Title("Dependencies")]
        [SerializeField] private CharacterSettings settings;
        [SerializeField] private CharacterController characterController;
    
        private float _speedAdditionalEffectsMultiplier;
        public float SpeedAdditionalEffectsMultiplier
        {
            get => _speedAdditionalEffectsMultiplier;
            set
            {
                _speedAdditionalEffectsMultiplier = value;
                if (_speedAdditionalEffectsMultiplier <= 0)
                    _speedAdditionalEffectsMultiplier = 1;
            }
        }
        
        private bool _isGamepad;
        private Camera _camera;
        private PlayerInputs _playerInputs;

        private Vector2 movementInput;
        private Vector3 rawMovement;
        private Vector3 characterFacingDirection;
        private Vector2 aim;
    
        private bool _isMovementPressed;
        private bool _isRunPressed;

        private bool isInvulnerable;

        private bool _isDashing;
        private bool isDashBuffered;

        private float dashElapsedTime = 0;

        
        private bool _isInputEnable;

        private void Awake()
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.CharacterControls.Move.started += HandleMovementByInput;
            _playerInputs.CharacterControls.Move.canceled += HandleMovementByInput;
            _playerInputs.CharacterControls.Move.performed += HandleMovementByInput;
        
            _playerInputs.CharacterControls.Run.started += HandleRun;
            _playerInputs.CharacterControls.Run.canceled += HandleRun;

            _playerInputs.CharacterControls.Dash.started += Dash;

            _playerInputs.CharacterControls.Action.started += TryInteraction;
            
            _camera = Camera.main;
        
            _isInputEnable = true;

            SpeedAdditionalEffectsMultiplier = 1;
        }

        private void OnEnable()
        {
            _playerInputs.Enable();
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
        }

        private void Update()
        {
            if (_isDashing)
                return;
        
            HandleRotationByInput();
            
            Vector3 movement = GetCameraAffectedMovement();
           
            // Note: Gravity
            movement += new Vector3(0f, -20f * Time.deltaTime, 0f);
            
            if (_isRunPressed)
            {
                characterController.Move(movement * (settings.speed * settings.runMultiplier * SpeedAdditionalEffectsMultiplier * Time.deltaTime));
                return;
            }
            
            characterController.Move(movement * (settings.speed * SpeedAdditionalEffectsMultiplier * Time.deltaTime));
        }
        
        public void OnDeviceChange(PlayerInput playerInput)
        {
            _isGamepad = playerInput.currentControlScheme.Equals("ProController") ? true : false;
            _isGamepad = playerInput.currentControlScheme.Equals("ProController") ? true : false;
        }
    
        private void HandleMovementByInput(InputAction.CallbackContext context)
        {
            if (!_isInputEnable)
                return;
            
            movementInput = context.ReadValue<Vector2>();
            rawMovement.x = movementInput.x;
            rawMovement.z = movementInput.y;
            _isMovementPressed = rawMovement.x != 0 || rawMovement.y != 0;
        }
        private void HandleRun(InputAction.CallbackContext context)
        {
            _isRunPressed = context.ReadValueAsButton();
        }
        private void HandleRotationByInput()
        {
            aim = _playerInputs.CharacterControls.Aim.ReadValue<Vector2>();
        
            if (_isGamepad)
            {
                if (Mathf.Abs(aim.x) > 0.01f || Mathf.Abs(aim.y) > 0.01f)
                {
                    Vector3 characterDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

                    if (characterDirection.sqrMagnitude > 0.0f)
                    {
                        Quaternion newRotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * Quaternion.LookRotation(characterDirection, Vector3.up);
                        transform.rotation =
                            Quaternion.RotateTowards(transform.rotation, newRotation, settings.rotationFactorPerFrame * Time.deltaTime);
                    
                        characterFacingDirection = transform.TransformDirection(Vector3.forward);
                    }
                }
            }
            else
            {
                Ray ray = _camera.ScreenPointToRay(aim);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                if (groundPlane.Raycast(ray, out var rayDistance))
                {
                    Vector3 point = ray.GetPoint(rayDistance);
                    LookAt(point);
                }
            }
        }
        
        private void LookAt(Vector3 point)
        {
            Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);

            // debugMouseAimObject.transform.position = heightCorrectedPoint;
            characterFacingDirection = (heightCorrectedPoint - transform.position).normalized;

            transform.LookAt(heightCorrectedPoint);
        }
        
        private void Dash(InputAction.CallbackContext obj)
        {
            if (_isDashing)
            {
                if (settings.bufferedDashPercentage <= dashElapsedTime / settings.dashDuration)
                    isDashBuffered = true;
                return;
            }
        
            _isDashing = true;
            StartCoroutine(DashCoroutine());
        }
    
        private IEnumerator DashCoroutine()
        {
            characterController.detectCollisions = false;

            dashElapsedTime = 0;
           
            Vector3 movementVector = transform.forward;
            if (_isMovementPressed)
                movementVector = GetCameraAffectedMovement();
            
            yield return null;
        
            while (dashElapsedTime <= settings.dashDuration )
            {
                var dashPercentage = dashElapsedTime / settings.dashDuration;
                isInvulnerable = false;
                
                if (dashPercentage >= settings.dashInvulnerabilityRange.x && dashPercentage <= settings.dashInvulnerabilityRange.y)
                {
                    isInvulnerable = true;
                }
                
                characterController.Move(movementVector * (settings.dashSpeed  * Time.deltaTime));
                dashElapsedTime += Time.deltaTime;
                yield return null;
            }

            _isDashing = false;
            characterController.detectCollisions = true;
            isInvulnerable = false;

            if (!isDashBuffered) yield break;
        
            _isDashing = true;
            isDashBuffered = false;
        
            StartCoroutine(DashCoroutine());
        }
    
        private void TryInteraction(InputAction.CallbackContext obj)
        {
            var colliders = Physics.OverlapSphere(transform.position, 2f, settings.interactionLayerMask);

            if (colliders.Length <= 0)
            {
                return;
            }
            
            var interactables = colliders
                .Select(t => t.GetComponent<IInteractable>())
                .Where(interactable => interactable != null)
                .ToList();

            foreach (var interactable in interactables)
            {
                interactable.Interact();
            }
            
            foreach (var interactable in interactables)
                interactable.Interact();
        }
        
        private Vector3 GetCameraAffectedMovement()
        {
            if (!_isInputEnable)
                return rawMovement;
            
            return Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * rawMovement;
        }
        
    }
}