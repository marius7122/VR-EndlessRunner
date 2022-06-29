using System;
using Locomotion.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Locomotion
{
    public class ArmSwingLocomotionProvider : LocomotionProvider
    {
        public event Action OnJump;

        [SerializeField] private InputActionReference leftEnable;
        [SerializeField] private InputActionReference rightEnable;

        [Header("Dependencies")] 
        [SerializeField] private ArmSwingLocomotionParametersSO parameters;
        [SerializeField] private XRControllerMovementInfoBehavior leftControllerMovement;
        [SerializeField] private XRControllerMovementInfoBehavior rightControllerMovement;

        private CharacterController _characterController;
        private Vector3 _verticalSpeed;
        private Vector3 _horizontalSpeed;
        private bool _lastFrameWasGrounded = true;
        private float _landingTime;

        private bool TrackLeftController => leftEnable.action.IsPressed();
        private bool TrackRightController => rightEnable.action.IsPressed();

        private void Start()
        {
            _characterController = system.xrOrigin.GetComponent<CharacterController>();

            if (_characterController == null)
            {
                Debug.LogError("[ArmSwingLocomotion] Character controller was not found on XROrigin; TODO: one will be added");
            }
        }
    
        private void Update()
        {
            ComputeArmMovement();
            ComputeArmJump();

            MoveRig();
        }
    
        private void ComputeArmMovement()
        {
            if (!_characterController.isGrounded && !parameters.canMoveInAir)
                return;
        
            var leftSpeed = TrackLeftController ? leftControllerMovement.Speed : Vector3.zero;
            var rightSpeed = TrackRightController ? rightControllerMovement.Speed : Vector3.zero;

            var controllersSpeed = leftSpeed.magnitude + rightSpeed.magnitude;
            var moveSpeed = controllersSpeed / parameters.handSpeedForMaxSpeed;
            moveSpeed = Mathf.Clamp01(moveSpeed);
            moveSpeed *= parameters.maxSpeed;
            
            var moveDirection = ControllersAverageFront();
            
            if ((moveDirection * moveSpeed).sqrMagnitude > _horizontalSpeed.sqrMagnitude)
            {
                _horizontalSpeed = moveDirection * moveSpeed;
            }
        }

        private void ComputeArmJump()
        {
            var leftAcceleration = TrackLeftController ? leftControllerMovement.Acceleration.y : 0f;
            var rightAcceleration = TrackRightController ? rightControllerMovement.Acceleration.y : 0f;
        
            if (leftAcceleration >= parameters.individualAccelerationNeededForJump &&
                rightAcceleration >= parameters.individualAccelerationNeededForJump &&
                leftAcceleration + rightAcceleration >= parameters.accelerationNeededForJump && 
                _characterController.isGrounded)
            {
                OnJump?.Invoke();
                _verticalSpeed = Vector3.up * parameters.jumpPower;
            }
        }

        private void MoveRig()
        {
            var movement = (_verticalSpeed + _horizontalSpeed) * Time.deltaTime;
            if (CanBeginLocomotion() && BeginLocomotion())
            {
                _characterController.Move(movement);

                EndLocomotion();
            }

            ApplyForces();
        }

        private Vector3 ControllersAverageFront()
        {
            var leftControllerFront = leftControllerMovement.Forward;
            leftControllerFront.y = 0f;
            // controller may be pointing up and not have a big factor in horizontal plane movement
            leftControllerFront.Normalize();
            
            var rightControllerFront = rightControllerMovement.Forward;
            rightControllerFront.y = 0f;
            rightControllerFront.Normalize();

            return (leftControllerFront + rightControllerFront).normalized;
        }

        private void ApplyForces()
        {
            ApplyGravity();
            DampenHorizontalSpeed();
        }

        private void ApplyGravity()
        {
            if (!parameters.useGravity) return;
            
            if (!_characterController.isGrounded)
                _verticalSpeed += Physics.gravity * Time.deltaTime;
            else
            {
                _verticalSpeed = Physics.gravity * 0.1f; // keep object grounded
            }
        }
        

        private void DampenHorizontalSpeed()
        {
            if(_characterController.isGrounded)
            {
                if (!_lastFrameWasGrounded)
                {
                    _landingTime = Time.time;
                }

                if (!Mathf.Approximately(_horizontalSpeed.y, 0))
                {
                    Debug.LogWarning($"Horizontal Speed Y is != 0: {_horizontalSpeed.y}");
                }

                var stoppingForce = -_horizontalSpeed;
                var stoppingSpeed = 
                    stoppingForce.normalized * (parameters.constantGroundDecelerationSpeed * Time.deltaTime) +
                    stoppingForce * (parameters.groundDecelerationFactor * Time.deltaTime);

                stoppingSpeed += stoppingSpeed * parameters.landingStoppingFactor.Evaluate(Time.time - _landingTime);

                var newHorizontalSpeed = _horizontalSpeed + stoppingSpeed;


                // did we started go backwards?
                var speedDot = Vector3.Dot(_horizontalSpeed, newHorizontalSpeed);
                if (speedDot <= 0f)
                    newHorizontalSpeed = Vector3.zero;

                _horizontalSpeed = newHorizontalSpeed;
            }

            _lastFrameWasGrounded = _characterController.isGrounded;
        }
    }
}
