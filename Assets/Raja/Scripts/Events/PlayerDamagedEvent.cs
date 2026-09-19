public struct PlayerDamagedEvent
{
    public float Damage { get; }
    public float CurrentHealth { get; }

    public PlayerDamagedEvent(float damage, float currentHealth)
    {
        Damage = damage;
        CurrentHealth = currentHealth;
    }
}