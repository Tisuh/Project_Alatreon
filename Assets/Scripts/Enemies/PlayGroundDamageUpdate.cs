using UnityEngine;
using UnityEngine.UI;

public class PlayGroundDamageUpdate : EnemyHealth
{
    [SerializeField] Text dmgText;

    private int dmgDone;

    protected override void Start()
    {
        base.Start();
        dmgText.text = "0";
    }

    public override void TakeDamage(int damage, elementType damageElement = elementType.neutral)
    {
        base.TakeDamage(damage, damageElement);

        dmgDone = maxHealth - currentHealth;
        dmgText.text = dmgDone.ToString();
    }
}
