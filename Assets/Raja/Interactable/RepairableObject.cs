
using UnityEngine;
using UnityEngine.Events;

public class RepairableObject : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int repairAmount = 25;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private UnityEvent onRepairRequested;
    [SerializeField] private UnityEvent<int, int> onHealthChanged;

    private bool playerInRange;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        NotifyHealthChanged();
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            RequestRepair();
        }
    }

    public void RequestRepair()
    {
        if (currentHealth >= maxHealth)
            return;

        onRepairRequested?.Invoke();
    }

    // Panggil method ini setelah biaya repair berhasil dibayar.
    public void Repair()
    {
        Repair(repairAmount);
    }

    public void Repair(int amount)
    {
        if (amount <= 0 || currentHealth >= maxHealth)
            return;

        currentHealth = Mathf.Min(
            currentHealth + amount,
            maxHealth
        );

        NotifyHealthChanged();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        currentHealth = Mathf.Max(
            currentHealth - damage,
            0
        );

        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}