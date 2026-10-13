using UnityEngine;
using TMPro;

public abstract class WeaponController : MonoBehaviour
{
    [Header("Main Class Stats")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int minAmmo;
    [SerializeField] private int damage;
    [SerializeField] private int rpm = 600;

    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    
    protected int currentAmmo;
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
        if (Input.GetMouseButton(0) && Time.time >= timeToShoot)
        {
            timeToShoot = Time.time + timeBetweenShots;
            currentAmmo -= 1;
            ammoCounter.text = currentAmmo.ToString();

            Shoot();
        }
    }

    public abstract void Shoot();
}