using System;
using UnityEngine;

namespace Common
{
    public class CharacterCollisionDetector : MonoBehaviour
    {
        public event Action<Vector3> OnImpact;

        private CharacterController _characterController;
        private CollisionFlags _lastFrameCollisions;
        private Vector3 _lastFrameVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (_characterController == null)
            {
                Debug.LogWarning($"[{nameof(CharacterCollisionDetector)}] needs a CharacterController");
            }
        }

        private void Update()
        {
            var currentCollisions = _characterController.collisionFlags;
            var currentVelocity = _characterController.velocity;
            if (currentCollisions > _lastFrameCollisions)
            {
                OnImpact?.Invoke(_lastFrameVelocity - currentVelocity);
            }

            _lastFrameCollisions = currentCollisions;
            _lastFrameVelocity = currentVelocity;
        }
    }
}
