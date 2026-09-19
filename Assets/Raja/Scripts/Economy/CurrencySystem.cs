using System;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance { get; private set; }

    [Header("Currency Settings")]
    [SerializeField, Min(0)] private int startingCandy = 0;

    private int currentCandy;

    public int CurrentCandy => currentCandy;

    public event Action<int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentCandy = startingCandy;
    }

    public bool CanAfford(int amount)
    {
        return amount >= 0 && currentCandy >= amount;
    }

    public bool AddCurrency(int amount)
    {
        if (amount <= 0)
            return false;

        currentCandy += amount;
        OnCurrencyChanged?.Invoke(currentCandy);

        return true;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0 || !CanAfford(amount))
            return false;

        currentCandy -= amount;
        OnCurrencyChanged?.Invoke(currentCandy);

        return true;
    }

    public void SetCurrency(int amount)
    {
        currentCandy = Mathf.Max(0, amount);
        OnCurrencyChanged?.Invoke(currentCandy);
    }
}