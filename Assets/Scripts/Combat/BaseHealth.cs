using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int minHealth = 0;
    [SerializeField] protected int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    protected abstract void Death();
}
