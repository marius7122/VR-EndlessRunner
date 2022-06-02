using System;
using Events.ScriptableObjects;
using Locomotion.Utils.SmoothProperty;
using UnityEngine;

namespace Enemies.Projectile
{
    public class ProjectileTarget : MonoBehaviour
    {
        public Vector3 Speed => _smoothSpeed.SmoothValue;

        [SerializeField] private VoidEventChannelSO playerWasHitChannel;
        
        private readonly SmoothProperty _smoothSpeed = new SmoothProperty(5);
        private Vector3 _lastFramePosition;

        private void Start()
        {
            _lastFramePosition = transform.position;
        }

        private void Update()
        {
            ComputeFrameSpeed();
        }
        

        public bool IsInFrontOf(Vector3 point)
        {
            var dirToPoint = (point - transform.position).normalized;
            var dot = Vector3.Dot(Speed.normalized, dirToPoint);
            // Debug.Log(dot);
            return dot < 0f;
        }

        private void ComputeFrameSpeed()
        {
            var currentPosition = transform.position;
            var deltaMovement = currentPosition - _lastFramePosition;
            var speed = deltaMovement / Time.deltaTime;
            _smoothSpeed.AddFrameRecord(speed);

            _lastFramePosition = currentPosition;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");
        }
    }
}