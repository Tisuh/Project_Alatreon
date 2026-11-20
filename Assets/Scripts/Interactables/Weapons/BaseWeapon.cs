using UnityEngine;
using TMPro;

public abstract class BaseWeapon : BasePickUp
{
    [Header("Stats")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int minAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int damage;
    [SerializeField] private int rpm = 600;
    [SerializeField] protected int weaponRange = 100;

    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    
    protected float timeToShoot;
    protected float timeBetweenShots;
    protected Camera cam;



    private void Start()
    {
        timeBetweenShots = 60f / rpm;
        cam = Camera.main;
        currentAmmo = Random.Range(minAmmo, maxAmmo);
        ammoCounter.text = currentAmmo.ToString();
    }

    private void Update()
    {
        
    }



    protected virtual void Shoot()
    {
        timeToShoot = Time.time + timeBetweenShots;
        currentAmmo -= 1;
        ammoCounter.text = currentAmmo.ToString();
    }

    public override void TryToShoot()
    {
        if (Time.time >= timeToShoot && currentAmmo > 0)
        {
            Shoot();
        }
    }
}