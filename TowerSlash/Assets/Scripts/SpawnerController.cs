using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Singleton<SpawnerController>
{
    // This would be your prefab to spawn
    [SerializeField] private GameObject enemyPrefab;

    // The transform position to spawn the enemies
    [SerializeField] private Transform spawnPoint;

    // To keep track of spawned enemies (optional)
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void StartGame()
    {
        // Start spawning enemies or reset the spawner
        SpawnEnemy();
    }

    public void Reset()
    {
        // Reset the spawner (destroy all enemies or reset counters)
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
    }
}
