using UnityEngine;

public class EnemyHealth : BaseHealth
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
