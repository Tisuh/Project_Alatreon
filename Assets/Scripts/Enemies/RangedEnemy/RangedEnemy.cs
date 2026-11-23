using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileOrigin;

    protected override void Attack()
    {
        Vector3 projectileDirectionToTarget = chaseTarget.position - projectileOrigin.position;
        EnemyProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation).GetComponent<EnemyProjectile>();

        projectile.ShootAtTarget(projectileDirectionToTarget);
    }
}
