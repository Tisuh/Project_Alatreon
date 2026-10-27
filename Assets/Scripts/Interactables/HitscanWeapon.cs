using UnityEngine;

public class HitscanWeapon : WeaponController
{
    [Header("HitScan Refs")]
    [SerializeField] private LayerMask enemyLayer;



    public override void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        Physics.Raycast(ray, out RaycastHit hit, weaponRange);

        Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.peru, 1);
        Debug.Log($"Has diparado ha: {hit.collider}");
    }
}
