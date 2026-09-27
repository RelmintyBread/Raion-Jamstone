/// <summary>
/// Interface untuk objek yang bisa menerima damage (Enemy, Knowledge, dll).
/// Implementasikan ini di script HealthSystem atau Enemy-mu.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
}
