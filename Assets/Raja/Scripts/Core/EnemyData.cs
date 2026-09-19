using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Detergentnation/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Information")]
    [SerializeField] private string enemyName;
    [SerializeField] private string description;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Reward")]
    [SerializeField] private int candyReward = 1;

    public string EnemyName => enemyName;
    public string Description => description;
    public Sprite EnemySprite => enemySprite;
    public GameObject EnemyPrefab => enemyPrefab;

    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    public int CandyReward => candyReward;
}