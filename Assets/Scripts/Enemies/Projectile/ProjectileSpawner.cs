using System.Collections;
using UnityEngine;

namespace Enemies.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private ProjectileTarget projectileTarget;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private ProjectilePropertiesSO projectileProperties;
        [SerializeField] private float shootCooldown = 5f;
        [SerializeField] private float shootingRange = 20f;
        [SerializeField] private float minimumShootingDistance = 10f;
        [SerializeField] private bool autoDestroy = true;

        private void Start()
        {
            StartCoroutine(ShootProjectilesCoroutine());
        }

        private void Update()
        {
            if (projectileTarget != null)
                transform.LookAt(projectileTarget.transform);
        }

        public void SetTarget(ProjectileTarget target)
        {
            projectileTarget = target;
        }

        public void SetProjectileProperties(ProjectilePropertiesSO newProperties)
        {
            projectileProperties = newProperties;
        }

        private IEnumerator ShootProjectilesCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(shootCooldown);

                if (projectileTarget == null)
                    continue;
                
                if (autoDestroy && projectileTarget.IsInFrontOf(transform.position))
                {
                    Destroy(gameObject);
                    yield break;
                }

                var distance = (projectileTarget.transform.position - transform.position).magnitude;
                if (distance > shootingRange || distance < minimumShootingDistance)
                    continue;

                var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newProjectile.Init(projectileTarget);
                newProjectile.SetProperties(projectileProperties);
            }
        }
        
    }
}
