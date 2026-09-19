using UnityEngine;

public enum SpawnLocation
{
    Default,
    Left,
    Right,
    Top,
    Bottom,
    Air,
    Ground
}

public enum SpawnCondition
{
    Immediate,
    Delay,
    Timer,
    WaveStart,
    PreviousGroupDefeated
}

[CreateAssetMenu(fileName = "NewWaveSpawnData", menuName = "Game/Wave Spawn Data")]
[System.Serializable]
public class WaveSpawnData : ScriptableObject
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private int amount;
    [SerializeField] private SpawnLocation location;
    [SerializeField] private SpawnCondition condition;
    [SerializeField] private float countdown;
    [SerializeField] private float spawnDelay;
}