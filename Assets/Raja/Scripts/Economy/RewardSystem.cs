using System;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public event Action<int> OnRewardGranted;

    public bool GiveReward(int amount)
    {
        if (amount <= 0)
            return false;

        if (CurrencySystem.Instance == null)
            return false;

        bool success = CurrencySystem.Instance.AddCurrency(amount);

        if (!success)
            return false;

        OnRewardGranted?.Invoke(amount);

        return true;
    }

    public bool GiveEnemyReward(Enemy enemy)
    {
        if (enemy == null)
            return false;

        return GiveReward(enemy.CandyReward);
    }
}