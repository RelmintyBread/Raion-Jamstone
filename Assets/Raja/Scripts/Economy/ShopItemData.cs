using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Detergentnation/Economy/Shop Item"
)]
public class ShopItemData : ScriptableObject
{
    [Header("Item Information")]
    [SerializeField] private string itemName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [Header("Purchase Settings")]
    [SerializeField, Min(0)] private int price = 10;
    [SerializeField] private GameObject itemPrefab;

    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public int Price => price;
    public GameObject ItemPrefab => itemPrefab;
}