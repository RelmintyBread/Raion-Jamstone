using UnityEngine;

public struct UpgradeSuccessEvent
{
    public Object UpgradedItem { get; }
    public int NewLevel { get; }
    public int UpgradeCost { get; }

    public UpgradeSuccessEvent(
        Object upgradedItem,
        int newLevel,
        int upgradeCost)
    {
        UpgradedItem = upgradedItem;
        NewLevel = newLevel;
        UpgradeCost = upgradeCost;
    }
}