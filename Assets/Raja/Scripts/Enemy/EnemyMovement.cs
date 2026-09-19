using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool moveTowardsTarget = true;

    [Header("References")]
    [SerializeField] private EnemyTargetting targetting;

    private Rigidbody2D rb;
    private Enemy enemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();

        if (targetting == null)
            targetting = GetComponent<EnemyTargetting>();
    }

    private void FixedUpdate()
    {
        if (!moveTowardsTarget)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (enemy != null && enemy.IsDead)
            return;

        if (targetting == null || !targetting.HasTarget)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        Transform target = targetting.Target;

        float directionX = Mathf.Sign(
            target.position.x - transform.position.x
        );

        rb.linearVelocity = new Vector2(
            directionX * moveSpeed,
            rb.linearVelocity.y
        );

        // Membalik arah visual berdasarkan arah gerak.
        if (directionX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * directionX;
            transform.localScale = scale;
        }
    }

    public void SetMovement(bool canMove)
    {
        moveTowardsTarget = canMove;

        if (!canMove)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Max(0f, newSpeed);
    }
}