using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            other.GetComponent<BaseHealth>().TakeDamage(9999999);
        }
        else
        {
            Destroy(other);
        }
    }
}
