using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private Transform respawnTf;

    protected override void Die()
    {
        Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnTf.position;
        transform.rotation = Quaternion.Euler(0, respawnTf.rotation.y, 0);
    }
}
