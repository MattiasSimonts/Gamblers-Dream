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
    }
    private void HandleMoving(float deltaTime)
    {
        _movementVelocity = Vector3.MoveTowards(_movementVelocity, Vector3.zero, _deccelartionSpeed * deltaTime);
        if (_input != Vector3.zero)
        {
            if (_isSprinting)
            {
                _movementVelocity += _sprintAccelartionSpeed * deltaTime * _input;
                _movementVelocity = Vector3.ClampMagnitude(_movementVelocity, _sprintSpeed);
            }
            else
            {
                _movementVelocity += _accelartionSpeed * deltaTime * _input;
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

        _playerCamera.transform.localRotation = Quaternion.Euler(_currentYAngle, 0f, 0f);

        _cameraHolder.transform.localRotation = Quaternion.Euler(new Vector3(_currentYAngle, 0, 0));
    }
}
