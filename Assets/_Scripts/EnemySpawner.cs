using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Waves Settings")]
    public int wavesCount = 2;
    public int enemiesPerWave = 5;
    public float spawnInterval = 3f;
    public float timeBetweenWaves = 5f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    
    IEnumerator SpawnWaves()
    {
        for (int wave = 0; wave < wavesCount; wave++)
        {
            GameManager.instance.ShowAdWave(wave + 1, wavesCount);
            yield return new WaitForSeconds(3f);

            int spawnedInWave = 0;
            while (spawnedInWave < enemiesPerWave)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
                spawnedInWave++;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return new WaitUntil(() => GameManager.instance.enemiesAlive <= 0);
        GameManager.instance.Victory();
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        GameManager.instance.UpdateEnemiesCounter(1);
    }
}
