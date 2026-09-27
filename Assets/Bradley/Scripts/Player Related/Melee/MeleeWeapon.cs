using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    #region Variables

    [Header("Melee Animation")]
    [Tooltip("Nama animation clip yang dimainkan saat menyerang, harus sama persis dengan nama clip di Animation window.")]
    [SerializeField] private string hitAnimationName = "Hit";

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
    }

    // Tidak perlu override Update lagi khusus rotasi senjata —
    // untuk side scroller, arah hadap sudah cukup ditangani lewat flip X di BaseWeapon.

    #endregion

    #region Protected Methods

    protected override void Shoot()
    {
        // Jangan memanggil base.Shoot() karena senjata jarak dekat tidak menembakkan proyektil/bullet

        if (weaponAnimator != null)
        {
            // Play langsung by name, TIDAK butuh parameter/transition di Animator Controller.
            // Cukup ada 1 clip bernama "Hit" (atau sesuai hitAnimationName) di Controller-nya.
            weaponAnimator.Play(hitAnimationName, 0, 0f);
        }

        // Mencegah spam klik dengan mengatur cooldown berdasarkan FireRate dari WeaponData
        if (weaponData != null)
        {
            nextFireTime = Time.time + weaponData.FireRate;
        }
        else
        {
            nextFireTime = Time.time + 0.5f; // Fallback jika WeaponData belum diisi
        }
    }

    #endregion
}