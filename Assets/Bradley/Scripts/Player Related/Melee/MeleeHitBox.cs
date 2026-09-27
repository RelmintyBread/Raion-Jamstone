using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    #region Variables

    [Header("Melee Hit Settings")]
    [SerializeField] private GameObject hitEffect; // Efek ledakan/percikan saat mengenai target

    private BaseWeapon parentWeapon;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Mengambil referensi senjata di parent untuk mendapatkan data Damage
        parentWeapon = GetComponentInParent<BaseWeapon>();
        if (parentWeapon == null)
        {
            Debug.LogWarning("[MeleeHitBox] BaseWeapon tidak ditemukan di parent object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            int damage = 10; // Default damage
            if (parentWeapon != null && parentWeapon.CurrentWeaponData != null)
            {
                damage = parentWeapon.CurrentWeaponData.Damage;
            }

            damageable.TakeDamage(damage);

            // Munculkan efek ledakan tepat di posisi musuh yang terkena hit
            SpawnHitEffect(other.ClosestPoint(transform.position));
        }
    }

    #endregion

    #region Private Methods

    private void SpawnHitEffect(Vector2 hitPosition)
    {
        if (hitEffect != null)
        {
            GameObject fx = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            Destroy(fx, 1f);
        }
    }

    #endregion
}
