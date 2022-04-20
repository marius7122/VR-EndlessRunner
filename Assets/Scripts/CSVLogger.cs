using System;
using System.IO;
using Locomotion;
using Locomotion.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class CSVLogger : MonoBehaviour
{
    [Header("Customization")] 
    [SerializeField] private string csvFolderPath = "./Logging/";
    [SerializeField] private string csvDataFile = "speed";
    [SerializeField] private string csvJumpTimeFile = "jump";
    [SerializeField] private string csvCustomEventTimeFile = "event";
    [SerializeField] private InputActionReference eventInput;

    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ArmSwingLocomotionProvider armSwingLocomotion;
    [SerializeField] private XRControllerMovementInfo leftController;
    [SerializeField] private XRControllerMovementInfo rightController;

    private StreamWriter _dataFile;
    private StreamWriter _jumpFile;
    private StreamWriter _eventFile;
    
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

        var path = csvFolderPath + DateTime.Now.ToString("MM-dd hh-mm-ss")  + "/";
        Directory.CreateDirectory(path);
        
        _dataFile = new StreamWriter(path + csvDataFile + ".csv");
        _dataFile.WriteLineAsync("time,speed,horizontal_speed,vertical_speed," +
                                  "left_speed,right_speed,left_acc,right_acc," +
                                  "x_left_speed,y_left_speed,z_left_speed,x_left_acc,y_left_acc,z_left_acc," +
                                  "x_right_speed,y_right_speed,z_right_speed,x_right_acc,y_right_acc,z_right_acc");
        
        
        _jumpFile = new StreamWriter(path + csvJumpTimeFile + ".csv");
        _jumpFile.WriteLineAsync("time");
        
        _eventFile = new StreamWriter(path + csvCustomEventTimeFile + ".csv");
        _eventFile.WriteLineAsync("time");

        armSwingLocomotion.OnJump += LogJump;
        eventInput.action.performed += LogEvent;
    }

    private void OnDestroy()
    {
        _dataFile.Close();
        _jumpFile.Close();
        _eventFile.Close();
        armSwingLocomotion.OnJump -= LogJump;
        eventInput.action.performed -= LogEvent;
    }

    private void Update()
    {
        var speed = characterController.velocity;
        var horizontalSpeed = speed;
        horizontalSpeed.y = 0;
        var verticalSpeed = speed.y;
        
        _dataFile.WriteLineAsync($"{Time.time}," +
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

    private void LogEvent(InputAction.CallbackContext context)
    {
        _eventFile.WriteLineAsync($"{Time.time}");
    }

    private string Vector3ToCSV(Vector3 vec)
    {
        return $"{vec.x},{vec.y},{vec.z}";
    }
}