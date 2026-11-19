using System.Collections;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.1f;

    protected override void Attack()
    {
        attackHitbox.SetActive(true);
        StartCoroutine(DeactivateHitboxCrt());
    }

    IEnumerator DeactivateHitboxCrt()
    {
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);
    }
}
