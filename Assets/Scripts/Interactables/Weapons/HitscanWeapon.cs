using UnityEngine;

public class HitscanWeapon : BaseWeapon
{
    [Header("HitScan Refs")]
    [SerializeField] private LayerMask affectedLayers;

    protected override void Shoot()
    {
        base.Shoot();

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.peru, 1);
        Debug.Log("antes del raycast " + ray);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange, affectedLayers))
        {
            Debug.Log("depues del raycast");
            if (hit.collider.TryGetComponent<BaseHealth>(out BaseHealth health))
            {
                Debug.Log("impacto registrado");

                health.TakeDamage(damage);
            }
        }
    }
}
