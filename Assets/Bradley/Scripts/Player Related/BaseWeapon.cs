using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    #region Variables

    [Header("Weapon Settings")]
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Transform firePoint;

    [Header("Visuals")]
    [SerializeField] protected SpriteRenderer defaultSpriteRenderer;
    [SerializeField] protected Animator weaponAnimator;

    protected float nextFireTime = 0f;
    protected PlayerMovement playerMovement;

    public bool IsEquipped { get; set; } = false;

    public WeaponData CurrentWeaponData => weaponData;

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        if (firePoint == null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name.Equals("FirePoint", System.StringComparison.OrdinalIgnoreCase))
                {
                    firePoint = child;
                    break;
                }
            }
        }

        if (firePoint == null)
        {
            Debug.LogWarning("[BaseWeapon] FirePoint not found on this weapon.");
        }

        if (weaponAnimator == null)
        {
            weaponAnimator = GetComponent<Animator>();
        }

        playerMovement = GetComponentInParent<PlayerMovement>();

        UpdateVisualState();
    }

    protected virtual void Update()
    {
        if (!IsEquipped)
        {
            return;
        }

        FlipToFaceDirection();

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    // Membalik tampilan senjata (flip local scale X) mengikuti arah hadap Player.
    // Karena ini di BaseWeapon, otomatis berlaku untuk Pistol DAN Bat tanpa perlu ditulis ulang.
    protected virtual void FlipToFaceDirection()
    {
        if (playerMovement == null) return;

        float dirX = playerMovement.LastMovementDirection.x;
        if (Mathf.Abs(dirX) < 0.01f) return; // diam persis di tengah, jangan flip (hindari kedip)

        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);
        scale.x = dirX < 0 ? -absX : absX;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEquipped)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponentInChildren<PlayerInventory>()
                                     ?? other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.PickupWeapon(this);
            }
        }
    }

    #endregion

    #region Public Methods

    public void Setup(WeaponData data)
    {
        weaponData = data;
    }

    public virtual void EquipTo(Transform targetParent)
    {
        transform.SetParent(targetParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        IsEquipped = true;

        // Memastikan playerMovement di-update (menggantikan SendMessage("Awake"))
        playerMovement = GetComponentInParent<PlayerMovement>();

        UpdateVisualState();
    }

    public virtual void Unequip()
    {
        transform.SetParent(null);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }

        IsEquipped = false;
        UpdateVisualState();
    }

    #endregion

    #region Protected Methods

    protected virtual void UpdateVisualState()
    {
        if (defaultSpriteRenderer != null)
        {
            defaultSpriteRenderer.enabled = !IsEquipped;
        }

        if (weaponAnimator != null)
        {
            weaponAnimator.enabled = IsEquipped;

            SpriteRenderer animSprite = weaponAnimator.GetComponent<SpriteRenderer>();
            if (animSprite != null && animSprite != defaultSpriteRenderer)
            {
                animSprite.enabled = IsEquipped;
            }
        }
    }

    protected virtual void Shoot()
    {
        if (weaponData == null || weaponData.BulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("[BaseWeapon] Missing weapon data, bullet prefab, or firepoint.");
            return;
        }

        Vector2 direction = playerMovement != null ? playerMovement.LastMovementDirection : Vector2.right;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bulletObject = Instantiate(weaponData.BulletPrefab, firePoint.position, bulletRotation);

        if (bulletObject != null)
        {
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.InitializeBullet(weaponData.BulletSpeed, weaponData.Damage);
            }
        }

        nextFireTime = Time.time + weaponData.FireRate;
    }

    #endregion
}