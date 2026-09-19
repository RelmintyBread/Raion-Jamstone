using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [SerializeField] private int waveNumber;
    [SerializeField] private WaveSpawnData[] spawns;
    [SerializeField] private bool hugeWave;
}