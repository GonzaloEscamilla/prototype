using UnityEngine;
using UnityEngine.InputSystem;

using Sirenix.OdinInspector;

using _Project.Scripts.Utilities;

namespace _Project.Scripts.Core
{
    public class TopDownCharacterKinematicController : MonoBehaviour
    {
        [Title("Dependencies")]
        [SerializeField] private CharacterSettings settings;
        [SerializeField] private CharacterMovementController movementController;
        [SerializeField] private DetectorSettings interactablesDetectorSettings;
        [SerializeField] private Transform interactablesDetectorCenter;
        [SerializeField] private CharacterWeaponController weaponController;
        
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

        public bool IsDashing => movementController.IsDashing;
        
        private bool _isGamepad;
        private Camera _camera;
        private PlayerInputs _playerInputs;
        private Detector<BaseInteractable> _interactablesDetector;
        private BaseInteractable _currentInteractableTarget;

        private Vector2 _movementInput;
        private Vector3 _characterFacingDirection;
        private Vector2 _aim;
    
        private bool _isRunPressed;
        private bool _isInteractionPressed;
        
        private bool _isInvulnerable;

        private bool _isInputEnable;
    
        private void Awake()
        {
            _camera = Camera.main;

            _playerInputs = new PlayerInputs();
            _playerInputs.Enable();

            _isInputEnable = true;
            SpeedAdditionalEffectsMultiplier = 1;
            
            _playerInputs.CharacterControls.Move.started += HandleMovementByInput;
            _playerInputs.CharacterControls.Move.canceled += HandleMovementByInput;
            _playerInputs.CharacterControls.Move.performed += HandleMovementByInput;
        
            _playerInputs.CharacterControls.Run.started += HandleRun;
            _playerInputs.CharacterControls.Run.canceled += HandleRun;

            _playerInputs.CharacterControls.Dash.started += Dash;

            _playerInputs.CharacterControls.Action.started += HandleInteraction;
            _playerInputs.CharacterControls.Action.canceled += HandleInteraction;

            _playerInputs.CharacterControls.Attack.started += AttackStarted;
            
            _interactablesDetector = new Detector<BaseInteractable>(interactablesDetectorCenter, interactablesDetectorSettings);
            _interactablesDetector.OnTargetDetected += InteractableTargetDetected;
            _interactablesDetector.OnTargetLost += InteractableTargetLost;
        }
        
        private void OnDisable()
        {
            _playerInputs.Disable();
        }

        private void Update()
        {
            if (IsDashing)
                return;
            
            _interactablesDetector.Update();
            TryToInteract();
            
            var moveData = new MoveInputData(
                _movementInput,
                _movementInput,
                _isRunPressed
            );

            movementController.SetInputs(moveData);
        }

        public void OnDeviceChange(PlayerInput playerInput)
        {
            _isGamepad = playerInput.currentControlScheme.Equals("ProController") ? true : false;
        }
    
        private void HandleMovementByInput(InputAction.CallbackContext context)
        {
            if (!_isInputEnable)
                return;
            
            _movementInput = context.ReadValue<Vector2>();
        }
        private void HandleRun(InputAction.CallbackContext context)
        {
            _isRunPressed = context.ReadValueAsButton();
        }
        
        private void Dash(InputAction.CallbackContext obj)
        {
            if (!_isInputEnable && !weaponController.IsAttacking)
                return;
            
            movementController.Dash();
        }
    
        private void InteractableTargetDetected(BaseInteractable interactable)
        {
            _currentInteractableTarget = interactable;
        }
        
        private void InteractableTargetLost(BaseInteractable interactableLost)
        {
            if (interactableLost != _currentInteractableTarget)
                return;
            
            _currentInteractableTarget = null;
        }
        
        private void HandleInteraction(InputAction.CallbackContext context)
        {
            _isInteractionPressed = context.ReadValueAsButton();
        }
        
        private void AttackStarted(InputAction.CallbackContext context)
        {
            if (!_isInputEnable)
                return;
            
            weaponController.Attack();
        }
        
        private void TryToInteract()
        {
            if (!_isInteractionPressed)
                return;
            
            _currentInteractableTarget?.Interact(gameObject);
        }
    }
}