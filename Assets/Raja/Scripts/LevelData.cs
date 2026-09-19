using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private string levelName;
    [SerializeField] private int startingDefensePoint;
    [SerializeField] private WaveData[] waves;
}