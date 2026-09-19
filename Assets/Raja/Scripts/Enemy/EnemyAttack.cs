using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private bool canAttack = true;

    [Header("References")]
    [SerializeField] private EnemyTargetting targetting;
    [SerializeField] private EnemyMovement movement;

    private float attackTimer;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        if (targetting == null)
            targetting = GetComponent<EnemyTargetting>();

        if (movement == null)
            movement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (!canAttack || (enemy != null && enemy.IsDead))
            return;

        if (targetting == null || !targetting.HasTarget)
        {
            if (movement != null)
                movement.SetMovement(false);

            return;
        }

        bool targetInRange = IsTargetInRange();

        if (movement != null)
            movement.SetMovement(!targetInRange);

        attackTimer -= Time.deltaTime;

        if (targetInRange && attackTimer <= 0f)
            Attack();
    }

    private bool IsTargetInRange()
    {
        Transform target = targetting.Target;

        if (target == null)
            return false;

        float distance = Vector2.Distance(
            transform.position,
            target.position
        );

        return distance <= attackRange;
    }

    private void Attack()
    {
        Transform target = targetting.Target;

        if (target == null)
            return;

        attackTimer = attackCooldown;

        // Menghentikan musuh saat melakukan serangan.
        if (movement != null)
            movement.SetMovement(false);

        target.gameObject.SendMessage(
            "TakeDamage",
            attackDamage,
            SendMessageOptions.DontRequireReceiver
        );
    }

    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }
}