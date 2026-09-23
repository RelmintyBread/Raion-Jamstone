using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public event Action<ShopItemData> OnItemPurchased;

    public bool CanPurchase(ShopItemData item)
    {
        if (item == null)
            return false;

        if (CurrencySystem.Instance == null)
            return false;

        return CurrencySystem.Instance.CanAfford(item.Price);
    }

    public bool Purchase(ShopItemData item)
    {
        if (!CanPurchase(item))
            return false;

        bool spent = CurrencySystem.Instance.SpendCurrency(item.Price);

        if (!spent)
            return false;

        OnItemPurchased?.Invoke(item);

        Debug.Log($"Purchased: {item.ItemName}");

        return true;
    }
}