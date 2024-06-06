using System;
using System.Collections;
using System.Linq;
using _Project.Scripts.Utilities;
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
        [SerializeField] private DetectorSettings interactablesDetectorSettings;
        
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
        private Detector<IInteractable> _interactablesDetected;

        private Vector2 _movementInput;
        private Vector3 _rawMovement;
        private Vector3 _characterFacingDirection;
        private Vector2 _aim;
    
        private bool _isMovementPressed;
        private bool _isRunPressed;

        private bool _isInvulnerable;

        private bool _isDashing;
        private bool _isDashBuffered;

        private float _dashElapsedTime = 0;

        
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

            _playerInputs.CharacterControls.Action.started += StartInteraction;
            _playerInputs.CharacterControls.Action.canceled += EndInteraction;
            
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
            
            _movementInput = context.ReadValue<Vector2>();
            _rawMovement.x = _movementInput.x;
            _rawMovement.z = _movementInput.y;
            _isMovementPressed = _rawMovement.x != 0 || _rawMovement.y != 0;
        }
        private void HandleRun(InputAction.CallbackContext context)
        {
            _isRunPressed = context.ReadValueAsButton();
        }
        private void HandleRotationByInput()
        {
            _aim = _playerInputs.CharacterControls.Aim.ReadValue<Vector2>();
        
            if (_isGamepad)
            {
                if (Mathf.Abs(_aim.x) > 0.01f || Mathf.Abs(_aim.y) > 0.01f)
                {
                    Vector3 characterDirection = Vector3.right * _aim.x + Vector3.forward * _aim.y;

                    if (characterDirection.sqrMagnitude > 0.0f)
                    {
                        Quaternion newRotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * Quaternion.LookRotation(characterDirection, Vector3.up);
                        transform.rotation =
                            Quaternion.RotateTowards(transform.rotation, newRotation, settings.rotationFactorPerFrame * Time.deltaTime);
                    
                        _characterFacingDirection = transform.TransformDirection(Vector3.forward);
                    }
                }
            }
            else
            {
                Ray ray = _camera.ScreenPointToRay(_aim);
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
            _characterFacingDirection = (heightCorrectedPoint - transform.position).normalized;

            transform.LookAt(heightCorrectedPoint);
        }
        
        private void Dash(InputAction.CallbackContext obj)
        {
            if (_isDashing)
            {
                if (settings.bufferedDashPercentage <= _dashElapsedTime / settings.dashDuration)
                    _isDashBuffered = true;
                return;
            }
        
            _isDashing = true;
            StartCoroutine(DashCoroutine());
        }
    
        private IEnumerator DashCoroutine()
        {
            characterController.detectCollisions = false;

            _dashElapsedTime = 0;
           
            Vector3 movementVector = transform.forward;
            if (_isMovementPressed)
                movementVector = GetCameraAffectedMovement();
            
            yield return null;
        
            while (_dashElapsedTime <= settings.dashDuration )
            {
                var dashPercentage = _dashElapsedTime / settings.dashDuration;
                _isInvulnerable = false;
                
                if (dashPercentage >= settings.dashInvulnerabilityRange.x && dashPercentage <= settings.dashInvulnerabilityRange.y)
                {
                    _isInvulnerable = true;
                }
                
                characterController.Move(movementVector * (settings.dashSpeed  * Time.deltaTime));
                _dashElapsedTime += Time.deltaTime;
                yield return null;
            }

            _isDashing = false;
            characterController.detectCollisions = true;
            _isInvulnerable = false;

            if (!_isDashBuffered) yield break;
        
            _isDashing = true;
            _isDashBuffered = false;
        
            StartCoroutine(DashCoroutine());
        }
    
        private void StartInteraction(InputAction.CallbackContext obj)
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
                interactable?.Interact(gameObject);
            }
        }
        
        private void EndInteraction(InputAction.CallbackContext obj)
        {
        }
        
        private Vector3 GetCameraAffectedMovement()
        {
            if (!_isInputEnable)
                return _rawMovement;
            
            return Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * _rawMovement;
        }
    }
}