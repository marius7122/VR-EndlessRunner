using System.Collections;
using UnityEngine;

namespace Enemies.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private ProjectileTarget projectileTarget;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private float shootCooldown = 5f;
        [SerializeField] private float shootingRange = 20f;

        private void Start()
        {
            StartCoroutine(ShootProjectilesCoroutine());
        }

        private IEnumerator ShootProjectilesCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(shootCooldown);
                
                if (projectileTarget.IsInFrontOf(transform.position))
                {
                    Destroy(gameObject);
                    yield break;
                }

                if (Vector3.SqrMagnitude(projectileTarget.transform.position - transform.position) >
                    shootingRange * shootingRange)
                    continue;

                var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newProjectile.SetTarget(projectileTarget);
                newProjectile.SetSpeed(projectileSpeed);
            }
        }
        
    }
}
