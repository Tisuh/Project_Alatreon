using UnityEngine;

public enum elementType
{
    neutral, fire, water, rock, lightning
}

public abstract class BaseHealth : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int minHealth = 0;
    [SerializeField] protected int currentHealth;

    [SerializeField] private elementType element;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, elementType damageElement = elementType.neutral)
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        float finalDamage = damage * DamageMultiplier(damageElement);

        currentHealth -= Mathf.RoundToInt(finalDamage);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    protected abstract void Death();

    float DamageMultiplier(elementType damageElement)
    {
        if (element == elementType.neutral || damageElement == elementType.neutral) return 1f; // Multiplicador si uno de los elementos es neutral

        if (element == damageElement) return 0.75f; // Multiplicador si los dos elementos son iguales

        switch (element)
        {
            case elementType.fire: // Si tu elemento es fuego
                return damageElement == elementType.water ? 1.25f : 1f;

            case elementType.water: // Si tu elemento es agua
                return damageElement == elementType.lightning ? 1.25f : 1f;

            case elementType.lightning: // Si tu elemento es rayo
                return damageElement == elementType.rock ? 1.25f : 1f;

            case elementType.rock: // Si tu elemento es roca
                return damageElement == elementType.fire ? 1.25f : 1f;

            default:
                return 1f;
        }
    }
}
