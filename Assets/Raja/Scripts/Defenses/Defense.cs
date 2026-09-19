using UnityEngine;

public abstract class Defense : MonoBehaviour
{
    [Header("Defense Data")]
    [SerializeField] protected DefenseData defenseData;

    protected float currentHealth;

    public DefenseData Data => defenseData;
    public float CurrentHealth => currentHealth;

    protected virtual void Awake()
    {
        if (defenseData != null)
        {
            Initialize(defenseData);
        }
    }

    public virtual void Initialize(DefenseData data)
    {
        defenseData = data;

        if (defenseData == null)
        {
            Debug.LogError("DefenseData belum di-assign!", this);
            return;
        }

        currentHealth = defenseData.MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0f || IsDestroyed())
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);

        if (currentHealth <= 0f)
        {
            DestroyDefense();
        }
    }

    public virtual void Repair(float amount)
    {
        if (amount <= 0f || IsDestroyed())
            return;

        currentHealth = Mathf.Min(
            currentHealth + amount,
            defenseData.MaxHealth
        );
    }

    public bool IsDestroyed()
    {
        return currentHealth <= 0f;
    }

    protected virtual void DestroyDefense()
    {
        Destroy(gameObject);
    }
}