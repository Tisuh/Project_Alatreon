using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    public void ShootAtTarget(Vector3 directionToTarget)
    {
        rb.AddForce(directionToTarget.normalized * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<BaseHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }
}
