using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private readonly Dictionary<UpgradeData, int> upgradeLevels = new();

    public event Action<UpgradeData, int, float> OnUpgradePurchased;

    public int GetUpgradeLevel(UpgradeData upgrade)
    {
        if (upgrade == null)
            return 0;

        return upgradeLevels.TryGetValue(upgrade, out int level)
            ? level
            : 0;
    }

    public bool IsMaxLevel(UpgradeData upgrade)
    {
        if (upgrade == null)
            return true;

        return GetUpgradeLevel(upgrade) >= upgrade.MaxLevel;
    }

    public int GetNextUpgradePrice(UpgradeData upgrade)
    {
        if (upgrade == null || IsMaxLevel(upgrade))
            return 0;

        return upgrade.GetPrice(GetUpgradeLevel(upgrade));
    }

    public bool CanPurchase(UpgradeData upgrade)
    {
        if (upgrade == null || IsMaxLevel(upgrade))
            return false;

        if (CurrencySystem.Instance == null)
            return false;

        int price = GetNextUpgradePrice(upgrade);

        return CurrencySystem.Instance.CanAfford(price);
    }

    public bool PurchaseUpgrade(UpgradeData upgrade)
    {
        if (!CanPurchase(upgrade))
            return false;

        int currentLevel = GetUpgradeLevel(upgrade);
        int price = upgrade.GetPrice(currentLevel);

        if (!CurrencySystem.Instance.SpendCurrency(price))
            return false;

        int newLevel = currentLevel + 1;
        upgradeLevels[upgrade] = newLevel;

        float totalValue = upgrade.GetTotalValue(newLevel);

        OnUpgradePurchased?.Invoke(
            upgrade,
            newLevel,
            totalValue
        );

        Debug.Log(
            $"{upgrade.UpgradeName} upgraded to level {newLevel}"
        );

        return true;
    }
}