using System.IO;
using UnityEngine;

public class CSVLogger : MonoBehaviour
{
    [SerializeField] private string csvSpeedPath = "speed.csv";
    [SerializeField] private string csvJumpPath = "jump.csv";
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ArmSwingLocomotionProvider armSwingLocomotion;
    [SerializeField] private XRControllerMovementInfo leftController;
    [SerializeField] private XRControllerMovementInfo rightController;

    private StreamWriter _speedFile;
    private StreamWriter _jumpFile;
    
    private void Awake()
    {
        characterController = characterController ? characterController : FindObjectOfType<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("[SpeedCSVLogger] No character controller found; component will not work");
        }

        if (leftController == null || rightController == null)
        {
            Debug.LogWarning("[SpeedCSVLogger] No ControllerMovementInfo component found; component will not work");
        }

        _speedFile = new StreamWriter(csvSpeedPath);
        _speedFile.WriteLineAsync("time,speed,horizontal_speed,vertical_speed," +
                                  "left_speed,right_speed,left_acc,right_acc," +
                                  "x_left_speed,y_left_speed,z_left_speed,x_left_acc,y_left_acc,z_left_acc," +
                                  "x_right_speed,y_right_speed,z_right_speed,x_right_acc,y_right_acc,z_right_acc");
        _jumpFile = new StreamWriter(csvJumpPath);
        _jumpFile.WriteLineAsync("time");

        armSwingLocomotion.OnJump += LogJump;
    }

    private void OnDestroy()
    {
        _speedFile.Close();
        _jumpFile.Close();
        armSwingLocomotion.OnJump -= LogJump;
    }

    private void Update()
    {
        var speed = characterController.velocity;
        var horizontalSpeed = speed;
        horizontalSpeed.y = 0;
        var verticalSpeed = speed.y;
        
        _speedFile.WriteLineAsync($"{Time.time}," +
                               $"{speed.magnitude},{horizontalSpeed.magnitude},{verticalSpeed}," +
                               $"{leftController.Speed.magnitude},{rightController.Speed.magnitude}," +
                               $"{leftController.Acceleration.magnitude},{rightController.Acceleration.magnitude}," +
                               $"{Vector3ToCSV(leftController.Speed)},{Vector3ToCSV(leftController.Acceleration)}," +
                               $"{Vector3ToCSV(rightController.Speed)},{Vector3ToCSV(rightController.Acceleration)}");
    }

    private void LogJump()
    {
        _jumpFile.WriteLineAsync($"{Time.time}");
    }

    private string Vector3ToCSV(Vector3 vec)
    {
        return $"{vec.x},{vec.y},{vec.z}";
    }
}