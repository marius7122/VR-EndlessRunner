using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XRControllerMovementInfoBehavior : MonoBehaviour
{
    public abstract Vector3 Speed { get; }
    
    public abstract Vector3 Acceleration { get; }
}

public class XRControllerMovementInfo : XRControllerMovementInfoBehavior
{
    [SerializeField] public float maxSpeed = 10f;
    
    public override Vector3 Speed => _speed;
    private Vector3 _speed;

    public override Vector3 Acceleration => _acceleration;
    private Vector3 _acceleration;

    private ActionBasedController _controller;
    private bool _lastPositionIsSet;
    private Vector3 _lastPosition = Vector3.zero;
    private Vector3 _lastSpeed =  Vector3.zero;

    private void Awake()
    {
        _controller = GetComponent<ActionBasedController>();
    }

    private void Update()
    {
        var currentPosition = transform.localPosition;
        if (_lastPositionIsSet)
        {
            var direction = _lastPosition - currentPosition;
            _speed = direction / Time.deltaTime;

            var speedDifference = _lastSpeed - _speed;
            _acceleration = speedDifference / Time.deltaTime;
        }
        
        _lastPosition = currentPosition;
        _lastSpeed = _speed;

        // Debug.Log($"{name.Split(' ')[0]}: {_controller.currentControllerState.inputTrackingState}");
    }
}
