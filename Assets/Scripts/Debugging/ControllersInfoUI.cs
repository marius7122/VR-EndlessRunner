using Locomotion.Utils;
using TMPro;
using UnityEngine;

namespace Debugging
{
    public class ControllersInfoUI : MonoBehaviour
    {
        [SerializeField] private bool alsoDebugInfo = true;
        [SerializeField] private TMP_Text infoText;
        [Header("Controllers")]
        [SerializeField] private XRControllerMovementInfo leftControllerInfo;
        [SerializeField] private XRControllerMovementInfo rightControllerInfo;
        [SerializeField] private CharacterController characterController;

        private void Update()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            var leftSpeed = leftControllerInfo.Speed;
            var rightSpeed = rightControllerInfo.Speed;
            leftSpeed.y = 0;
            rightSpeed.y = 0;

            var leftAcc = leftControllerInfo.Acceleration;
            var rightAcc = rightControllerInfo.Acceleration;
        
            infoText.text = $"IsGrounded: <color={(characterController.isGrounded ? "#000000" : "#00ff00")}>{characterController.isGrounded}</color>";

            if (alsoDebugInfo)
            {
                Debug.Log($"{infoText.text}");
            }
        }
    }
}
