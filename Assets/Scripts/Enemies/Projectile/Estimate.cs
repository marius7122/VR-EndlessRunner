using UnityEngine;

namespace Enemies.Projectile
{
    public static class Estimate
    {
        public static Vector3 Position(Vector3 startPosition, Vector3 speedVector, float afterSeconds)
        {
            return startPosition + speedVector * afterSeconds;
        }

        public static float TimeToReachPosition(Vector3 startPosition, Vector3 destination, float speed)
        {
            var distanceToPosition = Vector3.Distance(startPosition, destination);
            return distanceToPosition / speed;
        }
    }
}