using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArmSwingLocomotionProvider : LocomotionProvider
{
    public event Action OnJump;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("How much speed the character will loss per second if it's grounded")]
    [SerializeField] private float constantGroundDecelerationSpeed = 10f;
    [Tooltip("How much the speed will impact the breaking force")]
    [SerializeField] private float groundDecelerationFactor = 1f;

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

    protected void Start()
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
        var leftSpeed = leftControllerMovement.Speed;
        var rightSpeed = rightControllerMovement.Speed;
        leftSpeed.y = 0;
        rightSpeed.y = 0;

        float swingForce = (leftSpeed.magnitude + rightSpeed.magnitude) * movementSpeed;

        var cameraForward = system.xrOrigin.Camera.transform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        if ((cameraForward * swingForce).magnitude > _horizontalSpeed.magnitude)
        {
            _horizontalSpeed = cameraForward * swingForce;
            if (_horizontalSpeed.magnitude > maxSpeed)
                _horizontalSpeed = _horizontalSpeed.normalized * maxSpeed;
        }
    }

    private void CalculateArmJump()
    {
        var leftAcceleration = leftControllerMovement.Acceleration.y;
        var rightAcceleration = rightControllerMovement.Acceleration.y;
        
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

        var movement = _verticalSpeed * Time.deltaTime + _horizontalSpeed * Time.deltaTime;
        
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
    
}
