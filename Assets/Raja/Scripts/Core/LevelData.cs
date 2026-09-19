using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Information")]
    [SerializeField] private int levelNumber;
    [SerializeField] private string levelName;
    [SerializeField] private string sceneName;

    [Header("Level Configuration")]
    [SerializeField] private int startingDefensePoints;
    [SerializeField] private List<WaveData> waves = new List<WaveData>();

    public int LevelNumber => levelNumber;
    public string LevelName => levelName;
    public string SceneName => sceneName;
    public int StartingDefensePoints => startingDefensePoints;
    public IReadOnlyList<WaveData> Waves => waves;
}