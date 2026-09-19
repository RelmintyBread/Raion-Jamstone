using UnityEngine;

public struct HealthChangedEvent
{
    public GameObject Target { get; }
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public HealthChangedEvent(
        GameObject target,
        float currentHealth,
        float maxHealth)
    {
        Target = target;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}