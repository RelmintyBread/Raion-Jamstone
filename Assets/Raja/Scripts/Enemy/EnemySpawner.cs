using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject enemyPrefab;
        public int spawnCount = 1;
        public float spawnInterval = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private List<SpawnEntry> spawnEntries = new();
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawner Settings")]
    [SerializeField] private bool spawnOnStart = false;
    [SerializeField] private int maxAliveEnemies = 20;
    [SerializeField] private float startDelay = 0f;

    private readonly List<Enemy> aliveEnemies = new();

    public int AliveEnemyCount => aliveEnemies.Count;

    private void Start()
    {
        if (spawnOnStart)
            StartCoroutine(SpawnRoutine());
    }

    public void StartSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        foreach (SpawnEntry entry in spawnEntries)
        {
            if (entry.enemyPrefab == null)
                continue;

            for (int i = 0; i < entry.spawnCount; i++)
            {
                CleanupDeadEnemies();

                while (aliveEnemies.Count >= maxAliveEnemies)
                {
                    CleanupDeadEnemies();
                    yield return null;
                }

                SpawnEnemy(entry.enemyPrefab);

                if (entry.spawnInterval > 0f)
                    yield return new WaitForSeconds(entry.spawnInterval);
            }
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Transform spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner: Belum ada spawn point.", this);
            return;
        }

        GameObject instance = Instantiate(
            prefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Enemy enemy = instance.GetComponent<Enemy>();

        if (enemy != null)
        {
            aliveEnemies.Add(enemy);
            enemy.OnEnemyDied += HandleEnemyDied;
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return null;

        List<Transform> validPoints = new();

        foreach (Transform point in spawnPoints)
        {
            if (point != null)
                validPoints.Add(point);
        }

        if (validPoints.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validPoints.Count);
        return validPoints[randomIndex];
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDied;
        aliveEnemies.Remove(enemy);
    }

    private void CleanupDeadEnemies()
    {
        aliveEnemies.RemoveAll(enemy => enemy == null);
    }
}