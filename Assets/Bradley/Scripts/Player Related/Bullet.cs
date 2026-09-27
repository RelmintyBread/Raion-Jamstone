using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    [SerializeField] protected float lifeTime = 3f;

    protected Rigidbody2D bulletRb;
    protected float bulletSpeed;
    protected int bulletDamage;
    protected bool isInitialized = false;

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();

        if (bulletRb == null)
        {
            Debug.LogError("[Bullet] Rigidbody2D not found on Bullet prefab.");
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        TryDealDamage(other.gameObject);
        Destroy(gameObject);
    }

    #endregion

    #region Public Methods

    public virtual void InitializeBullet(float speed, int damage)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        isInitialized = true;

        LaunchBullet();
    }

    #endregion

    #region Protected Methods

    protected virtual void LaunchBullet()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[Bullet] Bullet launched without initialization.");
            return;
        }

        if (bulletRb == null)
        {
            return;
        }

        bulletRb.linearVelocity = transform.right * bulletSpeed;
    }

    protected virtual void TryDealDamage(GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        damageable.TakeDamage(bulletDamage);
        Debug.Log($"[Bullet] Hit: {target.name} for {bulletDamage} damage.");
    }

    #endregion
}