using UnityEngine;

public struct PurchaseEvent
{
    public Object PurchasedItem { get; }
    public int Price { get; }

    public PurchaseEvent(Object purchasedItem, int price)
    {
        PurchasedItem = purchasedItem;
        Price = price;
    }
}