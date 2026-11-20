using UnityEngine;

public class EnemyHealth : BaseHealth
{
    protected override void Death()
    {
        Destroy(gameObject);
    }
}
