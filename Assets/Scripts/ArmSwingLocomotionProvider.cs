using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ArmSwingLocomotionProvider : LocomotionProvider
{
    public event Action OnJump;

    [SerializeField] private InputActionReference leftEnable;
    [SerializeField] private InputActionReference rightEnable;

    [Header("Movement")]
    [SerializeField] private float handSpeedForMaxSpeed = 3f;
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("How much speed the character will loss per second if it's grounded")]
    [SerializeField] private float constantGroundDecelerationSpeed = 10f;
    [Tooltip("How much the speed will impact the breaking force")]
    [SerializeField] private float groundDecelerationFactor = 1f;
    [SerializeField] private bool canMoveInAir = false;

    [Header("Jump")]
    [SerializeField] private bool useGravity = true;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float accelerationNeededForJump = 200f;
    [SerializeField] private float individualAccelerationNeededForJump = 70f;
    
    [Header("Dependencies")]
    [SerializeField] private XRControllerMovementInfoBehavior leftControllerMovement;
    [SerializeField] private XRControllerMovementInfoBehavior rightControllerMovement;

    private CharacterController _characterController;
    private Vector3 _verticalSpeed;
    private Vector3 _horizontalSpeed;

    private bool TrackLeftController => leftEnable.action.IsPressed();
    private bool TrackRightController => rightEnable.action.IsPressed();

    private void Start()
    {
        _characterController = system.xrOrigin.GetComponent<CharacterController>();

        if (_characterController == null)
        {
            Debug.LogError("[ArmSwingLocomotion] Character controller was not found on XROrigin; TODO: one will be added");
            // TODO: add a character controller (+ character driver)
        }
    }
    
    private void Update()
    {
        CalculateArmMovement();
        CalculateArmJump();

        MoveRig();
    }
    
    private void CalculateArmMovement()
    {
        if (!_characterController.isGrounded && !canMoveInAir)
            return;
        
        var leftSpeed = TrackLeftController ? leftControllerMovement.Speed : Vector3.zero;
        var rightSpeed = TrackRightController ? rightControllerMovement.Speed : Vector3.zero;
        leftSpeed.y = 0;
        rightSpeed.y = 0;


        // HMD direction
        // var moveDirection = system.xrOrigin.Camera.transform.forward;
        // moveDirection.y = 0f;
        // moveDirection = moveDirection.normalized;
        
        var moveSpeedNormalized = Mathf.Clamp01((leftSpeed.magnitude + rightSpeed.magnitude) / handSpeedForMaxSpeed);
        var moveSpeed = moveSpeedNormalized * maxSpeed;
        var moveDirection = ControllersAverageFront();
        if ((moveDirection * moveSpeed).magnitude > _horizontalSpeed.magnitude)
        {
            _horizontalSpeed = moveDirection * moveSpeed;
        }
    }

    private void CalculateArmJump()
    {
        var leftAcceleration = TrackLeftController ? leftControllerMovement.Acceleration.y : 0f;
        var rightAcceleration = TrackRightController ? rightControllerMovement.Acceleration.y : 0f;
        
        if (leftAcceleration >= individualAccelerationNeededForJump &&
            rightAcceleration >= individualAccelerationNeededForJump &&
            leftAcceleration + rightAcceleration >= accelerationNeededForJump && 
            _characterController.isGrounded)
        {
            OnJump?.Invoke();
            _verticalSpeed = Vector3.up * jumpPower;
        }
    }

    private void MoveRig()
    {
        // apply gravity
        if (useGravity)
        {
            if (!_characterController.isGrounded)
                _verticalSpeed += Physics.gravity * Time.deltaTime;
            else
                _verticalSpeed += Physics.gravity * (Time.deltaTime * 0.1f);    // keep object grounded
        }

        var movement = (_verticalSpeed + _horizontalSpeed) * Time.deltaTime;

        if (CanBeginLocomotion() && BeginLocomotion())
        {
            _characterController.Move(movement);
            EndLocomotion();
        }
        
        // dampen horizontal speed
        if (_characterController.isGrounded)
        {
            if(!Mathf.Approximately(_horizontalSpeed.y, 0))
            {
                Debug.LogWarning($"Horizontal Speed Y is != 0: {_horizontalSpeed.y}");
            }
            
            var stoppingForce = -_horizontalSpeed;
            var newHorizontalSpeed = _horizontalSpeed + 
                                     stoppingForce.normalized * (constantGroundDecelerationSpeed * Time.deltaTime) + 
                                     stoppingForce * (groundDecelerationFactor * Time.deltaTime);
        
            // did we started go backwards?
            var speedDot = Vector3.Dot(_horizontalSpeed, newHorizontalSpeed);
            if (speedDot <= 0f)
                newHorizontalSpeed = Vector3.zero;

            _horizontalSpeed = newHorizontalSpeed;
        }
    }

    private Vector3 ControllersAverageFront()
    {
        var leftControllerFront = TrackLeftController ? leftControllerMovement.transform.forward : Vector3.zero;
        leftControllerFront.y = 0f;
        // controller may point up and not have a big factor in horizontal plane movement
        leftControllerFront.Normalize();
        
        var rightControllerFront = TrackRightController ? rightControllerMovement.transform.forward : Vector3.zero;
        rightControllerFront.y = 0f;
        rightControllerFront.Normalize();

        return (leftControllerFront + rightControllerFront).normalized;
    }
}
