using Assets.Scripts;
using Assets.Scripts.Player;
using DG.Tweening;
using System;
using JetBrains.Annotations;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private InputActionAsset _playerInputs;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _accelartionSpeed;
    [SerializeField] private float _deccelartionSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _sprintAccelartionSpeed;
    [SerializeField] private float _lookSens;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Vector2 _yRotationLimits;

    [SerializeField] private GameObject _leftClickToInteract;
    [SerializeField] private TextMeshProUGUI _leftClickToInteractText;

    [SerializeField] private float _normalFOV;
    [SerializeField] private float _sprintFOV;
    [SerializeField] private float _fovChangeSpeed;
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactionLayer;

    [SerializeField] private float _gravity;

    [SerializeField] private PlayerHP _playerHP;
    [SerializeField] private float _sprintHPUsagePerSecond;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioResource _pickupAudioResource;

    private float _startingHeight;
    [CanBeNull] private Collider _lastCollider;
    [CanBeNull] private IInteractable _lastInteractable;

    private float _currentYAngle;
    private CharacterController _characterController;
    private bool _isSprinting;
    private InputAction _look;
    private InputAction _move;
    private InputAction _interact;
    private InputAction _sprint;

    [SerializeField] private Vector3 _movementVelocity;


    [SerializeField] private Vector3 _input;
    [SerializeField] private TextMeshProUGUI _coinsDisplayText;
    private int _coins;

    public int Coins
    {
        get { return _coins; }
        set
        {
            if (value == _coins)
                return;
            _coins = value;

            UpdateCoinVisual();
        }
    }

    private void UpdateCoinVisual()
    {
        _coinsDisplayText.text = Coins.ToString();
    }

    [SerializeField] private int _startingCoins;
    private void Start()
    {
        Coins = _startingCoins;
        _startingHeight = transform.position.y;
    }
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _playerInputs.Enable();
        _look = _playerInputs.FindAction("Look");
        _move = _playerInputs.FindAction("Move");
        _interact = _playerInputs.FindAction("Interact");
        _sprint = _playerInputs.FindAction("Sprint");

        _look.performed += HandleLooking;
        _move.performed += MoveStarted;
        _move.canceled += MoveCanceled;

        _sprint.started += SprintStarted;
        _sprint.canceled += SprintCanceled;

        _interact.performed += Interact;

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out RaycastHit hitInfo, _interactionRange, _interactionLayer))
        {
            IInteractable hoveringInteractable = hitInfo.collider.GetComponent<IInteractable>();
            if (hoveringInteractable.CanInteract)
                hitInfo.collider.GetComponent<IInteractable>().OnInteract(this);
        }
    }
    public void AddCoins(int amount)
    {
        Coins += amount;

        _audioSource.resource = _pickupAudioResource;
        _audioSource.Play();
    }
    private void SprintCanceled(InputAction.CallbackContext obj)
    {
        _isSprinting = false;
    }

    private void SprintStarted(InputAction.CallbackContext obj)
    {
        _isSprinting = true;
    }

    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        _input = Vector2.zero;
    }

    private void MoveStarted(InputAction.CallbackContext obj)
    {
        Vector2 rawInput = obj.ReadValue<Vector2>().normalized;
        _input = new Vector3(rawInput.x, 0, rawInput.y);
    }
    private void FixedUpdate()
    {
        HandleMoving(Time.deltaTime);

        HandleFOV(Time.deltaTime);
        
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out RaycastHit hitInfo, _interactionRange, _interactionLayer))
        {
            if (_lastCollider is null && _lastInteractable is null && hitInfo.collider != _lastCollider)
            {
                IInteractable hoveringInteractable = hitInfo.collider.GetComponent<IInteractable>();
                _leftClickToInteract.SetActive(hoveringInteractable.CanInteract);

                _leftClickToInteractText.text = hoveringInteractable.InteractionName;

                _lastCollider = hitInfo.collider;
                _lastInteractable = hoveringInteractable;
            }
            else
                _leftClickToInteract.SetActive(_lastInteractable.CanInteract);
            
        }
        else
        {
            _leftClickToInteract.SetActive(false);
            _lastCollider = null;
            _lastInteractable = null;
        }

        transform.position = new Vector3(transform.position.x, _startingHeight, transform.position.z);
    }

    private void HandleFOV(float deltaTime)
    {
        float targetFOV = _isSprinting ? _sprintFOV : _normalFOV;

        if (_playerCamera.Lens.FieldOfView != targetFOV)
        {
            _playerCamera.Lens.FieldOfView = Mathf.MoveTowards(_playerCamera.Lens.FieldOfView, targetFOV, _fovChangeSpeed * Time.deltaTime);
        }
    }

    private void HandleMoving(float deltaTime)
    {
        _movementVelocity = Vector3.MoveTowards(_movementVelocity, Vector3.zero, _deccelartionSpeed * deltaTime);
        Vector3 input = transform.forward * _input.z + transform.right * _input.x;
        if (_input != Vector3.zero)
        {
            if (_isSprinting)
            {
                _movementVelocity += _sprintAccelartionSpeed * deltaTime * input;
                _movementVelocity = Vector3.ClampMagnitude(_movementVelocity, _sprintSpeed);

                _playerHP.Health -= _sprintHPUsagePerSecond * Time.deltaTime;
            }
            else
            {
                _movementVelocity += _accelartionSpeed * deltaTime * input;
                _movementVelocity = Vector3.ClampMagnitude(_movementVelocity, _moveSpeed);
            }
        }

        if (!_characterController.isGrounded)
        {
            _movementVelocity += Vector3.down * _gravity;
        }

        _characterController.Move(_movementVelocity);
    }

    private void HandleLooking(InputAction.CallbackContext obj)
    {
        Vector2 mouseDelta = obj.ReadValue<Vector2>();

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseDelta.x * _lookSens * Time.deltaTime, transform.rotation.eulerAngles.z);

        _currentYAngle -= mouseDelta.y * _lookSens * Time.deltaTime;

        _currentYAngle = Mathf.Clamp(_currentYAngle, _yRotationLimits.x, _yRotationLimits.y);

        _cameraHolder.localRotation = Quaternion.Euler(_currentYAngle, 0f, 0f);
    }
}