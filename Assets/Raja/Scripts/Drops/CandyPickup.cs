using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private int amount = 1;

    private bool isCollected;

    public void SetAmount(int value)
    {
        amount = Mathf.Max(0, value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected)
            return;

        if (!other.CompareTag("Player"))
            return;

        isCollected = true;

        CurrencySystem.Instance.AddCurrency(amount);

        Destroy(gameObject);
    }
}