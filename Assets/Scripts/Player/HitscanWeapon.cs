using Unity.VisualScripting;
using UnityEngine;

public class HitscanWeapon : WeaponController
{
    [SerializeField] private LayerMask enemyLayer = 7;

    [Header("HitScan Stats")]
    [SerializeField] private int range = 100;

    private Vector3 aimDirection;

    private void Start()
    {
        aimDirection = transform.TransformDirection(Vector3.forward);
    }

    public override void Shoot()
    {
        rpmTimer = timeBetweenShots;
        currentAmmo -= 1;

        bool RaycastHit = Physics.Raycast(bulletSpawnPoint.position, Vector3.forward, range, enemyLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.peru;
        Gizmos.DrawRay(bulletSpawnPoint.position, Vector3.forward * range);
    }
}
