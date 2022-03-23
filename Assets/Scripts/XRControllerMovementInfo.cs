using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XRControllerMovementInfoBehavior : MonoBehaviour
{
    public abstract Vector3 Speed { get; }
    
    public abstract Vector3 Acceleration { get; }
}

public class XRControllerMovementInfo : XRControllerMovementInfoBehavior
{
    public override Vector3 Speed => _speed;
    private Vector3 _speed;

    public override Vector3 Acceleration => _acceleration;
    private Vector3 _acceleration;

    private ActionBasedController _controller;
    private bool _controllerIsTracked;
    private Vector3 _lastPosition = Vector3.zero;
    private Vector3 _lastSpeed =  Vector3.zero;

    private void Awake()
    {
        _controller = GetComponent<ActionBasedController>();
    }

    private void Update()
    {
        UpdateControllerStats();
    }

    private void UpdateControllerStats()
    {
        var currentPosition = transform.localPosition;
        if (_controllerIsTracked)
        {
            var direction = _lastPosition - currentPosition;
            _speed = direction / Time.deltaTime;

            var speedDifference = _lastSpeed - _speed;
            _acceleration = speedDifference / Time.deltaTime;
        }
        else
        {
            _speed = Vector3.zero;
            _acceleration = Vector3.zero;
        }

        var trackingState = _controller.currentControllerState.inputTrackingState;
        _controllerIsTracked = trackingState.HasFlag(InputTrackingState.Position);
        if (_controllerIsTracked)
        {
            _lastPosition = currentPosition;
            _lastSpeed = _speed;
        }
    }
    
    
}
