using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Singleton<SpawnerController>
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform spawnPoint;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void StartGame()
    {
        SpawnEnemy();
    }

    public void Reset()
    {
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
