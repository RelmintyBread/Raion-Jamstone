using UnityEngine;

public class AmmoDefense : Defense
{
    private int currentAmmo;
    private float nextAttackTime;

    public int CurrentAmmo => currentAmmo;

    public override void Initialize(DefenseData data)
    {
        base.Initialize(data);

        if (defenseData == null)
            return;

        currentAmmo = defenseData.MaxAmmo;
        nextAttackTime = 0f;
    }

    public bool CanAttack()
    {
        return !IsDestroyed()
            && currentAmmo > 0
            && Time.time >= nextAttackTime;
    }

    public bool TryConsumeAmmo()
    {
        if (!CanAttack())
            return false;

        currentAmmo--;

        nextAttackTime =
            Time.time + defenseData.AttackCooldown;

        return true;
    }

    public void Reload()
    {
        if (defenseData == null || IsDestroyed())
            return;

        currentAmmo = defenseData.MaxAmmo;
    }
}