using Assets.Scripts;
using DG.Tweening;
using System;
using Unity.Cinemachine;
using UnityEngine;
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

    [SerializeField] private float _normalFOV;
    [SerializeField] private float _sprintFOV;
    [SerializeField] private float _fovChangeSpeed;
    [SerializeField] private float _interactionRange;
    [SerializeField] private LayerMask _interactionLayer;

    private float _currentYAngle;
    private CharacterController _characterController;
    private bool _isSprinting;
    private InputAction _look;
    private InputAction _move;
    private InputAction _interact;
    private InputAction _sprint;

    [SerializeField] private Vector3 _movementVelocity;


    [SerializeField] private Vector3 _input;

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
            hitInfo.collider.GetComponent<IInteractable>().OnInteract();
        }
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

        _leftClickToInteract.SetActive(Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, _interactionRange, _interactionLayer));

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
            }
            else
            {
                _movementVelocity += _accelartionSpeed * deltaTime * input;
                _movementVelocity = Vector3.ClampMagnitude(_movementVelocity, _moveSpeed);
            }
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