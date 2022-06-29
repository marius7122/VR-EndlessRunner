using UnityEngine;

namespace Level.Generation
{
    [CreateAssetMenu(fileName = "MovementParameters", menuName = "ScriptableObjects/Level/MovementParameters")]
    public class MovementParameters : ScriptableObject
    {
        public float maxSpeed;
        public float jumpSpeed;
        public float gravitationalAcceleration;

        public float MaxJumpDistance()
        {
            return jumpSpeed / gravitationalAcceleration * 2f * maxSpeed;
        }

        public float MaxHeightDifference(float jumpDistance)
        {
            var dt = jumpDistance / maxSpeed;
            return jumpSpeed * dt - 0.5f * gravitationalAcceleration * dt * dt;
        }
    }
}