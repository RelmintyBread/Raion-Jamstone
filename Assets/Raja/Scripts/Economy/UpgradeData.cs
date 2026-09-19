using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Detergentnation/Economy/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Upgrade Information")]
    [SerializeField] private string upgradeName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [Header("Upgrade Settings")]
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField, Min(0)] private int basePrice = 50;
    [SerializeField, Min(0)] private int priceIncreasePerLevel = 25;
    [SerializeField, Min(1)] private int maxLevel = 5;
    [SerializeField, Min(0f)] private float valuePerLevel = 1f;

    public string UpgradeName => upgradeName;
    public string Description => description;
    public Sprite Icon => icon;
    public UpgradeType Type => upgradeType;
    public int BasePrice => basePrice;
    public int PriceIncreasePerLevel => priceIncreasePerLevel;
    public int MaxLevel => maxLevel;
    public float ValuePerLevel => valuePerLevel;

    public int GetPrice(int currentLevel)
    {
        return basePrice + (priceIncreasePerLevel * currentLevel);
    }

    public float GetTotalValue(int level)
    {
        return valuePerLevel * level;
    }
}