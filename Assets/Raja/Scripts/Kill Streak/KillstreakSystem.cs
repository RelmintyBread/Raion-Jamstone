using System;
using System.Collections.Generic;
using UnityEngine;

public class KillstreakSystem : MonoBehaviour
{
    [Serializable]
    public class KillstreakMilestone
    {
        public int requiredKills = 3;
        public int candyBonus = 10;
    }

    [Header("Killstreak Settings")]
    [SerializeField, Min(0f)] private float streakResetTime = 5f;
    [SerializeField] private List<KillstreakMilestone> milestones = new();

    [Header("References")]
    [SerializeField] private RewardSystem rewardSystem;

    private readonly HashSet<Enemy> registeredEnemies = new();

    private int currentKillstreak;
    private float streakTimer;

    public int CurrentKillstreak => currentKillstreak;
    public float StreakTimer => streakTimer;

    public event Action<int> OnKillstreakChanged;
    public event Action<int, int> OnMilestoneReached;
    public event Action OnKillstreakReset;

    private void Update()
    {
        if (currentKillstreak <= 0)
            return;

        streakTimer -= Time.deltaTime;

        if (streakTimer <= 0f)
            ResetKillstreak();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null || registeredEnemies.Contains(enemy))
            return;

        registeredEnemies.Add(enemy);
        enemy.OnEnemyDied += HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        if (enemy == null || !registeredEnemies.Remove(enemy))
            return;

        enemy.OnEnemyDied -= HandleEnemyDied;

        AddKill();
    }

    private void AddKill()
    {
        currentKillstreak++;
        streakTimer = streakResetTime;

        OnKillstreakChanged?.Invoke(currentKillstreak);

        CheckMilestones();
    }

    private void CheckMilestones()
    {
        foreach (KillstreakMilestone milestone in milestones)
        {
            if (milestone == null)
                continue;

            if (currentKillstreak == milestone.requiredKills)
            {
                OnMilestoneReached?.Invoke(
                    currentKillstreak,
                    milestone.candyBonus
                );

                if (rewardSystem != null && milestone.candyBonus > 0)
                {
                    rewardSystem.GiveReward(
                        milestone.candyBonus
                    );
                }
            }
        }
    }

    public void ResetKillstreak()
    {
        if (currentKillstreak <= 0)
            return;

        currentKillstreak = 0;
        streakTimer = 0f;

        OnKillstreakChanged?.Invoke(currentKillstreak);
        OnKillstreakReset?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (Enemy enemy in registeredEnemies)
        {
            if (enemy != null)
                enemy.OnEnemyDied -= HandleEnemyDied;
        }

        registeredEnemies.Clear();
    }
}