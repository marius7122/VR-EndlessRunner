using UnityEngine;

namespace Enemies.Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        private Vector3 _direction;

        private void Update()
        {
            transform.position += _direction * (speed * Time.deltaTime);
        }

        public void SetTarget(ProjectileTarget target)
        {
            SetDirectionToHitTarget(target);
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        private void SetDirectionToHitTarget(ProjectileTarget target)
        {
            var collisionEta = EstimateCollisionEta(target);
            if (float.IsInfinity(collisionEta))
            {
                Destroy(gameObject);
                return;
            }
            
            var collisionPoint = Estimate.Position(target.transform.position, target.Speed, collisionEta);
            _direction = DirectionToPoint(collisionPoint);
            transform.LookAt(collisionPoint);
            
            Destroy(gameObject, collisionEta + 5f);
        }


        private float EstimateCollisionEta(ProjectileTarget target)
        {
            const float accuracy = 0.01f;
            var lowerBound = 0f;
            var upperBound = 100f;
            
            // target faster than projectile => projectile can't follow
            if (target.Speed.magnitude > speed)
            {
                var closestPoint = FindClosestPointOnTrajectory(target);
                if (Estimate.TimeToReachPosition(transform.position, closestPoint, speed) >
                    Estimate.TimeToReachPosition(target.transform.position, closestPoint, target.Speed.magnitude))
                    return float.PositiveInfinity;

                upperBound = Estimate.TimeToReachPosition(target.transform.position, closestPoint, speed);
            }

            int steps = 0;
            while (true)
            {
                var mid = (lowerBound + upperBound) * 0.5f;

                var targetPosition = Estimate.Position(target.transform.position, target.Speed, mid);
                
                var projectileSpeedVector = DirectionToPoint(targetPosition) * speed;
                var projectilePosition = transform.position + projectileSpeedVector * mid;

                var projectileError = Vector3.Distance(targetPosition, projectilePosition);

                if (projectileError <= accuracy)
                    return mid;

                var targetPointDistance = Vector3.Distance(transform.position, targetPosition);
                var traveledDistance = speed * mid;
                
                // not enough time to hit the target
                if (traveledDistance < targetPointDistance)
                    lowerBound = mid;
                else
                    upperBound = mid;

                steps++;
                if (steps == 1000)
                {
                    Debug.LogWarning($"Steps = 1000; lb = {lowerBound}; ub = {upperBound}");
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
    }
}