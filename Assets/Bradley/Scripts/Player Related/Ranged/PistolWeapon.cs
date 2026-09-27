using UnityEngine;

public class PistolWeapon : BaseWeapon
{
    #region Unity Methods

    private Vector2 lastDirection = Vector2.zero;
    private bool isAnimating = false;

    protected override void Update()
    {
        base.Update();

        if (IsEquipped && weaponAnimator != null && playerMovement != null)
        {
            Vector2 currentDir = playerMovement.LastMovementDirection;
            if (currentDir != lastDirection)
            {
                lastDirection = currentDir;

                if (currentDir.y > 0)
                {
                    weaponAnimator.Play("ShootUp", 0, 0f);
                }
                else if (currentDir.y < 0)
                {
                    weaponAnimator.Play("ShootDown", 0, 0f);
                }
                else
                {
                    weaponAnimator.Play("ShootSide", 0, 0f);
                }

                if (!isAnimating)
                {
                    weaponAnimator.speed = 0f;
                }
            }
        }
    }

    #endregion

    #region Protected Methods

    protected override void Shoot()
    {
        base.Shoot(); // BaseWeapon sudah menangani arah lurus dan men-spawn bullet

        if (weaponAnimator != null)
        {
            Vector2 direction = playerMovement != null ? playerMovement.LastMovementDirection : Vector2.right;

            if (direction.y > 0)
            {
                PlayAnimationAndPause("ShootUp");
            }
            else if (direction.y < 0)
            {
                PlayAnimationAndPause("ShootDown");
            }
            else
            {
                PlayAnimationAndPause("ShootSide");
            }
        }
    }

    private Coroutine animCoroutine;

    private void PlayAnimationAndPause(string stateName)
    {
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }
        animCoroutine = StartCoroutine(AnimRoutine(stateName));
    }

    private System.Collections.IEnumerator AnimRoutine(string stateName)
    {
        weaponAnimator.speed = 1f;
        weaponAnimator.Play(stateName, 0, 0f);

        // Tunggu 1 frame agar Animator mengupdate State Info-nya
        yield return null;

        // Dapatkan durasi asli dari animasi yang sedang dimainkan
        float length = weaponAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Tunggu sampai animasinya benar-benar selesai
        yield return new WaitForSeconds(length > 0f ? length : 0.1f);

        // Setelah selesai, pause di frame 0 sebagai "Idle"
        weaponAnimator.speed = 0f;
        weaponAnimator.Play(stateName, 0, 0f);
    }

    #endregion
}
