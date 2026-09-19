using UnityEngine;

[CreateAssetMenu(fileName = "NewDefenseData", menuName = "Detergentnation/Defense Data"
)]
public class DefenseData : ScriptableObject
{
    [Header("Defense Information")]
    [SerializeField] private string defenseName;
    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private DefenseType defenseType;
    [SerializeField] private GameObject defensePrefab;
    [SerializeField] private Sprite defenseSprite;

    [Header("Placement")]
    [SerializeField, Min(0)] private int defensePointCost = 1;

    [Header("Health")]
    [SerializeField, Min(1f)] private float maxHealth = 100f;

    [Header("Ammo Defense")]
    [SerializeField, Min(0)] private int maxAmmo = 10;
    [SerializeField, Min(0f)] private float attackDamage = 10f;
    [SerializeField, Min(0.1f)] private float attackCooldown = 1f;

    public string DefenseName => defenseName;
    public string Description => description;

    public DefenseType DefenseType => defenseType;
    public GameObject DefensePrefab => defensePrefab;
    public Sprite DefenseSprite => defenseSprite;

    public int DefensePointCost => defensePointCost;

    public float MaxHealth => maxHealth;

    public int MaxAmmo => maxAmmo;
    public float AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
}