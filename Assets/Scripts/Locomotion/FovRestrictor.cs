using System;
using UnityEngine;

namespace Locomotion
{
    public class FovRestrictor : MonoBehaviour
    {
        [SerializeField] private float distanceToCamera = 0.1f;
        [SerializeField] private float fovRadiusToScale = 2f;
        [SerializeField] private float startingFov = 150f;

        public float CurrentFOV { get; private set; }
        
        private void Start()
        {
            RestrictFov(startingFov);
        }

        public void RestrictFov(float targetAngle)
        {
            float alpha = targetAngle * 0.5f * Mathf.Deg2Rad;
            float targetCircleRadius = 2f * Mathf.Tan(alpha) * distanceToCamera;
            float scaleFactor = targetCircleRadius * fovRadiusToScale;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            CurrentFOV = targetAngle;
        }
        
        [ContextMenu("UpdateFov")]
        private void UpdateFov()
        {
            RestrictFov(startingFov);
        }
    }
}
