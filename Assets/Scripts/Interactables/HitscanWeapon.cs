using UnityEngine;

public class HitscanWeapon : WeaponController
{
    [Header("HitScan Refs")]
    [SerializeField] private LayerMask affectedLayers;

    protected override void Shoot()
    {
        base.Shoot();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, cam.nearClipPlane));

        Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.peru, 1);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange, affectedLayers))
        {
            if (hit.collider.TryGetComponent<BaseHealth>(out BaseHealth health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}
