using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private LevelData levelData;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;
    [SerializeField] private Transform[] topSpawnPoints;

    private readonly HashSet<Enemy> activeEnemies =
        new HashSet<Enemy>();

    private Coroutine waveRoutine;
    private int currentWaveIndex = -1;
    private int spawnedEnemyCount;

    private bool isRunning;

    public int CurrentWaveNumber { get; private set; }
    public int ActiveEnemyCount => activeEnemies.Count;
    public bool IsRunning => isRunning;

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    public void StartWaves()
    {
        if (isRunning)
            return;

        if (levelData == null)
        {
            Debug.LogError("LevelData belum di-assign!", this);
            return;
        }

        if (levelData.Waves.Count == 0)
        {
            Debug.LogWarning("Level ini belum memiliki WaveData.", this);
            return;
        }

        isRunning = true;
        currentWaveIndex = -1;

        waveRoutine = StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (int i = 0; i < levelData.Waves.Count; i++)
        {
            WaveData waveData = levelData.Waves[i];

            if (waveData == null)
            {
                Debug.LogWarning($"WaveData pada index {i} kosong.");
                continue;
            }

            currentWaveIndex = i;
            CurrentWaveNumber = waveData.WaveNumber;

            // Tunggu kondisi wave terpenuhi.
            yield return WaitForStartCondition(waveData);

            // Countdown sebelum spawn.
            if (waveData.CountdownBeforeWave > 0f)
            {
                yield return new WaitForSeconds(
                    waveData.CountdownBeforeWave
                );
            }

            Debug.Log($"Wave {CurrentWaveNumber} dimulai!");

            // Spawn seluruh kelompok enemy pada wave.
            yield return SpawnWave(waveData);

            Debug.Log($"Wave {CurrentWaveNumber} selesai spawn.");
        }

        // Tunggu sampai seluruh enemy dari wave terakhir dikalahkan.
        yield return new WaitUntil(() => activeEnemies.Count == 0);

        isRunning = false;
        waveRoutine = null;

        Debug.Log("Semua wave selesai!");
    }

    private IEnumerator WaitForStartCondition(WaveData waveData)
    {
        if (currentWaveIndex == 0)
            yield break;

        switch (waveData.StartCondition)
        {
            case WaveStartCondition.LevelStart:
                // Kondisi ini khusus wave yang dimulai saat level mulai.
                break;

            case WaveStartCondition.PreviousWaveFinishedSpawning:
                // RunWaves berjalan berurutan, jadi spawn wave sebelumnya
                // sudah selesai ketika kode sampai di sini.
                break;

            case WaveStartCondition.PreviousWaveAllDefeated:
                yield return new WaitUntil(
                    () => activeEnemies.Count == 0
                );
                break;
        }
    }

    private IEnumerator SpawnWave(WaveData waveData)
    {
        spawnedEnemyCount = 0;

        foreach (WaveSpawnData spawnData in waveData.Spawns)
        {
            if (spawnData == null || spawnData.EnemyData == null)
            {
                Debug.LogWarning(
                    "WaveSpawnData atau EnemyData belum diisi."
                );
                continue;
            }

            for (int i = 0; i < spawnData.Amount; i++)
            {
                SpawnEnemy(spawnData);

                spawnedEnemyCount++;

                if (waveData.SpawnInterval > 0f)
                {
                    yield return new WaitForSeconds(
                        waveData.SpawnInterval
                    );
                }
            }
        }
    }

    private void SpawnEnemy(WaveSpawnData spawnData)
    {
        GameObject prefab = spawnData.EnemyData.EnemyPrefab;

        if (prefab == null)
        {
            Debug.LogError(
                $"Prefab untuk {spawnData.EnemyData.EnemyName} belum diisi."
            );
            return;
        }

        Transform spawnPoint = GetSpawnPoint(
            spawnData.SpawnLocation
        );

        if (spawnPoint == null)
        {
            Debug.LogError(
                $"Spawn point {spawnData.SpawnLocation} belum tersedia.",
                this
            );
            return;
        }

        GameObject enemyObject = Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError(
                "Prefab enemy tidak memiliki component Enemy.",
                enemyObject
            );

            Destroy(enemyObject);
            return;
        }

        activeEnemies.Add(enemy);
    }

    private Transform GetSpawnPoint(SpawnLocation location)
    {
        switch (location)
        {
            case SpawnLocation.Left:
                return GetRandomPoint(leftSpawnPoints);

            case SpawnLocation.Right:
                return GetRandomPoint(rightSpawnPoints);

            case SpawnLocation.Top:
                return GetRandomPoint(topSpawnPoints);

            case SpawnLocation.Random:
                List<Transform> allPoints = new List<Transform>();

                AddPoints(allPoints, leftSpawnPoints);
                AddPoints(allPoints, rightSpawnPoints);
                AddPoints(allPoints, topSpawnPoints);

                return GetRandomPoint(allPoints.ToArray());

            default:
                return null;
        }
    }

    private Transform GetRandomPoint(Transform[] points)
    {
        if (points == null || points.Length == 0)
            return null;

        List<Transform> validPoints = new List<Transform>();

        foreach (Transform point in points)
        {
            if (point != null)
                validPoints.Add(point);
        }

        if (validPoints.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validPoints.Count);
        return validPoints[randomIndex];
    }

    private void AddPoints(
        List<Transform> target,
        Transform[] source
    )
    {
        if (source == null)
            return;

        foreach (Transform point in source)
        {
            if (point != null)
                target.Add(point);
        }
    }

    private void OnEnemyKilled(EnemyKilledEvent eventData)
    {
        if (eventData.Enemy == null)
            return;

        activeEnemies.Remove(eventData.Enemy);
    }

    public void StopWaves()
    {
        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);
            waveRoutine = null;
        }

        isRunning = false;
    }
}