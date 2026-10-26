using UnityEngine;
using TMPro;

public abstract class WeaponController : PickUp
{
    [Header("Stats")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int minAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] private int damage;
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

        Debug.Log(timeBetweenShots);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToShoot && currentAmmo > 0)
        {
            timeToShoot = Time.time + timeBetweenShots;
            currentAmmo -= 1;
            ammoCounter.text = currentAmmo.ToString();

            Shoot();
        }
    }

    public abstract void Shoot();
}