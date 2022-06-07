﻿using UnityEngine;

namespace Enemies.Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxCorrectionAngularSpeed = 5f;

        private Vector3 _direction;
        private ProjectileTarget _target;
        
        private void Update()
        {
            AdjustDirection();
            transform.position += _direction * (speed * Time.deltaTime);

            if (MissedTarget())
            {
                Destroy(gameObject);
            }
        }

        public void Init(ProjectileTarget target)
        {
            _target = target;
            var targetPosition = target.transform.position;
            transform.LookAt(targetPosition);
            _direction = DirectionToPoint(targetPosition);
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        private void AdjustDirection()
        {
            var directionToTarget = ComputeDirectionToHitTarget(_target, out _);

            var maxRadianCorrection = maxCorrectionAngularSpeed * Mathf.Deg2Rad * Time.deltaTime;
            _direction = Vector3.RotateTowards(_direction, directionToTarget, maxRadianCorrection, 0.0f);

            transform.LookAt(transform.position + _direction);
        }
        
        private Vector3 ComputeDirectionToHitTarget(ProjectileTarget target, out bool canHit)
        {
            var collisionEta = EstimateCollisionEta(target);
            if (float.IsInfinity(collisionEta))
            {
                canHit = false;
                return DirectionToPoint(target.transform.position);
            }
            
            var collisionPoint = Estimate.Position(target.transform.position, target.Speed, collisionEta);
            canHit = true;
            return DirectionToPoint(collisionPoint);
        }
        
        private float EstimateCollisionEta(ProjectileTarget target)
        {
            const float accuracy = 0.05f;
            var lowerBound = 0f;
            var upperBound = 100f;
            
            // target faster than projectile => projectile can't follow
            var currentPosition = transform.position;
            var currentTargetPosition = target.transform.position;
            if (target.Speed.magnitude > speed)
            {
                var closestPoint = FindClosestPointOnTrajectory(target);
                if (Estimate.TimeToReachPosition(currentPosition, closestPoint, speed) >
                    Estimate.TimeToReachPosition(currentTargetPosition, closestPoint, target.Speed.magnitude))
                    return float.PositiveInfinity;

                upperBound = Estimate.TimeToReachPosition(currentTargetPosition, closestPoint, speed);
            }

            const int maxSteps = 100;
            var currentStep = 0;
            while (true)
            {
                var mid = (lowerBound + upperBound) * 0.5f;

                
                var targetPosition = Estimate.Position(currentTargetPosition, target.Speed, mid);
                
                var projectileSpeedVector = DirectionToPoint(targetPosition) * speed;
                var projectilePosition = Estimate.Position(currentPosition, projectileSpeedVector, mid);

                var projectileError = Vector3.Distance(targetPosition, projectilePosition);
                
                // relative accuracy
                if (projectileError <= accuracy * Vector3.Distance(currentPosition, currentTargetPosition))
                    return mid;

                var targetPointDistance = Vector3.Distance(currentPosition, targetPosition);
                var traveledDistance = speed * mid;
                
                // not enough time to hit the target
                if (traveledDistance < targetPointDistance)
                    lowerBound = mid;
                else
                    upperBound = mid;
    
                
                // accuracy may never be good enough
                currentStep++;
                if (currentStep == maxSteps)
                {
                    Debug.LogWarning($"Steps = {maxSteps}; lb = {lowerBound}; ub = {upperBound}");
                    return float.PositiveInfinity;
                }
            }
        }

        private Vector3 FindClosestPointOnTrajectory(ProjectileTarget target)
        {
            var trajectory = target.Speed.normalized;
            var targetPosition = target.transform.position;
            var targetToProjectileDirection = transform.position - targetPosition;
            return Vector3.Project(targetToProjectileDirection, trajectory) + targetPosition;
        }

        private Vector3 DirectionToPoint(Vector3 targetPoint)
        {
            return (targetPoint - transform.position).normalized;
        }

        private bool MissedTarget()
        {
            var directionToTarget = Vector3.Normalize(_target.transform.position - transform.position);
            return Vector3.Dot(transform.forward, directionToTarget) < 0f;
        }
    }
}