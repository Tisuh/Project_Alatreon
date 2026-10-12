using UnityEngine;
using TMPro;

public abstract class WeaponController : MonoBehaviour
{
    [Header("Main Class Stats")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int minAmmo;
    [SerializeField] private int damage;
    [SerializeField] protected float timeBetweenShots;

    [Header("Refs")]
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] private TextMeshPro ammoCounter;
    
    protected int currentAmmo;
    protected float rpmTimer;

    private void Update()
    {
        if (rpmTimer > 0)
        {
            rpmTimer -= Time.deltaTime;
            return;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
    }

    public abstract void Shoot();
}