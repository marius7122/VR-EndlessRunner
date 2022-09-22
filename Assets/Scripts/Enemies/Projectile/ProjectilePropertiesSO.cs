using UnityEngine;

namespace Enemies.Projectile
{
    [CreateAssetMenu(fileName = "Projectile Properties", menuName = "ScriptableObjects/Enemies/ProjectileProperties")]
    public class ProjectilePropertiesSO : ScriptableObject
    {
        public float speed = 5f;
        public float maxCorrectionAngularSpeed = 5f;
    }
}