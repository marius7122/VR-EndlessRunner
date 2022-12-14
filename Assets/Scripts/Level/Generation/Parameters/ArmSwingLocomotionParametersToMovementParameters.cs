using Locomotion;
using UnityEngine;

namespace Level.Generation.Parameters
{
    [CreateAssetMenu(fileName = "MovementParameters", menuName = "ScriptableObjects/Level/ArmSwingParameters to MovementParameters")]
    public class ArmSwingLocomotionParametersToMovementParameters : PlayerMovementParameters
    {
        // TODO: a custom property drawer so that inherited fields are not visible
        [SerializeField] private ArmSwingLocomotionParametersSO armSwingParameters;

        private void OnEnable()
        {
            maxSpeed = armSwingParameters.maxSpeed;
            gravitationalAcceleration = Mathf.Abs(Physics.gravity.y);
            jumpSpeed = armSwingParameters.jumpPower;
        }
    }
}