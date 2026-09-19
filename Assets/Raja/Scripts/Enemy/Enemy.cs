using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int candyReward = 10;

    private int currentHealth;
    private bool isDead;

    public string EnemyName => enemyName;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int CandyReward => candyReward;
    public bool IsDead => isDead;

    public event Action<Enemy> OnEnemyDied;
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        OnEnemyDied?.Invoke(this);

        Destroy(gameObject);
    }
}