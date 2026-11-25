using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private Transform respawnTf;
    [SerializeField] private Image healthBarFiller;
    [SerializeField] private float healthBarUpdateSpeed = 5;

    protected override void Death()
    {
        Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnTf.position;
        transform.rotation = Quaternion.Euler(0, respawnTf.rotation.y, 0);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthBarNewValue = currentHealth * 0.01f;

        healthBarFiller.fillAmount = healthBarNewValue;
    }

    public override void TakeDamage(int damage, elementType damageElement = elementType.neutral)
    {
        base.TakeDamage(damage, damageElement);

        if (currentHealth > 0)
        {
            UpdateHealthBar();
        }
    }
}
