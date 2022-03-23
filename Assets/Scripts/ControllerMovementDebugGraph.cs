using UnityEngine;

public class ControllerMovementDebugGraph : MonoBehaviour
{
    [SerializeField] private XRControllerMovementInfo controller;

    // ACCELERATION
    [DebugGUIGraph(group: 1, min: -5, max: 5, autoScale: true)]
    public float XAcceleration => controller.Acceleration.x;
    
    
    [DebugGUIGraph(group: 2, min: -5, max: 5, autoScale: true)]
    public float YAcceleration => controller.Acceleration.y;
    
    [DebugGUIGraph(group: 3, min: -5, max: 5, autoScale: true)]
    public float ZAcceleration => controller.Acceleration.z;
    
    
    // SPEED
    [DebugGUIGraph(group: 4, min: -5, max: 5, autoScale: true)]
    public float XSpeed => controller.Speed.x;
    
    
    [DebugGUIGraph(group: 4, min: -5, max: 5, autoScale: true)]
    public float YSpeed => controller.Speed.y;
    
    [DebugGUIGraph(group: 4, min: -5, max: 5, autoScale: true)]
    public float ZSpeed => controller.Speed.z;

}
