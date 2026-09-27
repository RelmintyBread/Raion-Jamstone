using UnityEngine;

public class PistolBullet : Bullet
{
    #region Variables

    [Header("Pistol Effect")]
    [SerializeField] private GameObject hitEffect; // Efek ledakan/percikan saat mengenai target

    #endregion

    #region Protected Methods

    protected override void TryDealDamage(GameObject target)
    {
        base.TryDealDamage(target);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemies") && !other.CompareTag("Knowledge") && !other.CompareTag("Wall"))
        {
            return;
        }

        // Spawn efek
        SpawnHitEffect();

        base.OnTriggerEnter2D(other);
    }

    #endregion

    #region Private Methods

    private void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            GameObject fx = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(fx, 1f); // Otomatis hancurkan efek setelah 1 detik
        }
    }

    #endregion
}