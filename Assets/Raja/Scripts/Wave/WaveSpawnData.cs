using UnityEngine;

[System.Serializable]
public class WaveSpawnData
{
    [Header("Enemy")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField, Min(1)] private int amount = 1;

    [Header("Spawn Location")]
    [SerializeField] private SpawnLocation spawnLocation;

    public EnemyData EnemyData => enemyData;
    public int Amount => amount;
    public SpawnLocation SpawnLocation => spawnLocation;
}