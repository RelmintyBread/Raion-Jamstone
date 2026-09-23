
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public static DropSystem Instance { get; private set; }

    [Header("Drop Settings")]
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private int defaultCandyAmount = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DropCandy(Vector3 position)
    {
        DropCandy(position, defaultCandyAmount);
    }

    public void DropCandy(Vector3 position, int amount)
    {
        if (candyPrefab == null)
        {
            Debug.LogWarning("Candy Prefab belum di-assign!");
            return;
        }

        if (amount <= 0)
            return;

        GameObject candy = Instantiate(
            candyPrefab,
            position,
            Quaternion.identity
        );

        CandyPickup pickup = candy.GetComponent<CandyPickup>();

        if (pickup != null)
            pickup.SetAmount(amount);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}