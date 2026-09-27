using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    #region Variables

    [SerializeField] private string weaponName = "Default Weapon";
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject weaponPrefab;

    #endregion

    #region Properties

    public string WeaponName => weaponName;
    public float FireRate => fireRate;
    public float BulletSpeed => bulletSpeed;
    public int Damage => damage;
    public GameObject BulletPrefab => bulletPrefab;
    public GameObject WeaponPrefab => weaponPrefab;

    #endregion
}