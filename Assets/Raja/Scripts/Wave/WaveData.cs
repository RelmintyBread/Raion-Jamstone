using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Detergentnation/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Information")]
    [SerializeField, Min(1)] private int waveNumber = 1;
    [SerializeField] private bool isHugeWave;

    [Header("Wave Start")]
    [SerializeField] private WaveStartCondition startCondition;
    [SerializeField, Min(0f)] private float countdownBeforeWave = 0f;

    [Header("Spawn Settings")]
    [SerializeField, Min(0f)] private float spawnInterval = 1f;

    [Header("Enemy Spawns")]
    [SerializeField] private List<WaveSpawnData> spawns =
        new List<WaveSpawnData>();

    public int WaveNumber => waveNumber;
    public bool IsHugeWave => isHugeWave;
    public WaveStartCondition StartCondition => startCondition;
    public float CountdownBeforeWave => countdownBeforeWave;
    public float SpawnInterval => spawnInterval;
    public IReadOnlyList<WaveSpawnData> Spawns => spawns;
}