using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Locomotion.Utils
{
    public abstract class XRControllerMovementInfoBehavior : MonoBehaviour
    {
        public abstract Vector3 Speed { get; }
    
        public abstract Vector3 Acceleration { get; }
    }

    public class XRControllerMovementInfo : XRControllerMovementInfoBehavior
    {
        public override Vector3 Speed => ControllerIsTracked ? _speedSmooth.SmoothValue : Vector3.zero;
        private SmoothProperty.SmoothProperty _speedSmooth;

        public override Vector3 Acceleration => ControllerIsTracked ? _accelerationSmooth.SmoothValue : Vector3.zero;
        private SmoothProperty.SmoothProperty _accelerationSmooth;

        [SerializeField] private int framesToCountForSmoothing = 5;


        private ActionBasedController _controller;
        private Vector3 _lastPosition = Vector3.zero;
        private Vector3 _lastSpeed =  Vector3.zero;
        private bool _lastPositionIsSet;

        private bool ControllerIsTracked =>
            _controller.currentControllerState.inputTrackingState.HasFlag(InputTrackingState.Position);
    
        private void Awake()
        {
            _controller = GetComponent<ActionBasedController>();

            _speedSmooth = new SmoothProperty.SmoothProperty(framesToCountForSmoothing);
            _accelerationSmooth = new SmoothProperty.SmoothProperty(framesToCountForSmoothing);
        }

        private void Update()
        {
            UpdateControllerStats();
        }

        private void UpdateControllerStats()
        {
            var currentPosition = transform.localPosition;
            if (_lastPositionIsSet)
            {
                var direction = _lastPosition - currentPosition;
                var currentSpeed = direction / Time.deltaTime;
                _speedSmooth.AddFrameRecord(currentSpeed);

                var speedDifference = _lastSpeed - Speed;
                _accelerationSmooth.AddFrameRecord(speedDifference / Time.deltaTime);

                _lastSpeed = Speed;
            }

            if (ControllerIsTracked)
            {
                _lastPosition = currentPosition;
                _lastPositionIsSet = true;
            }
            else
            {
                _lastSpeed = Vector3.zero;
                _lastPositionIsSet = false;
            }
        }
    }
}