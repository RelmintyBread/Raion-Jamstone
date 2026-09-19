using UnityEngine;

public class HealthDefense : Defense
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Repair(float amount)
    {
        base.Repair(amount);
    }
}