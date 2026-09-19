using UnityEngine;

// Ditambahkan di prefab enemy

public class EnemyRewardHandler : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private RewardSystem rewardSystem;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        if (enemy != null)
            enemy.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        if (enemy != null)
            enemy.OnEnemyDied -= HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy deadEnemy)
    {
        if (rewardSystem != null)
            rewardSystem.GiveEnemyReward(deadEnemy);
    }
}