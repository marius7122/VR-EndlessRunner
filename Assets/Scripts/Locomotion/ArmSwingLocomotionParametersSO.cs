using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "LocomotionParameters", menuName = "ScriptableObjects/ArmSwingLocomotion/Parameters")]
    public class ArmSwingLocomotionParametersSO : ScriptableObject
    {
        [Header("Movement")]
        public float handSpeedForMaxSpeed = 3f;
        public float maxSpeed = 10f;
        [Tooltip("How much speed the character will loss per second if it's grounded")]
        public float constantGroundDecelerationSpeed = 10f;
        [Tooltip("How much the speed will impact the breaking force")]
        public float groundDecelerationFactor = 1f;
        public bool canMoveInAir;

        [Header("Jump")]
        public bool useGravity = true;
        public float jumpPower = 8f;
        public float accelerationNeededForJump = 200f;
        public float individualAccelerationNeededForJump = 70f;
        public AnimationCurve landingStoppingFactor;
    }
}