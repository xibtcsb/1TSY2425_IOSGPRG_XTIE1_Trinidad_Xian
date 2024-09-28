using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float enemyLifetime = 5f;

    [Header("Floor Settings")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private float floorHeight = 5f;
    [SerializeField] private int initialFloorCount = 5;

    private Queue<GameObject> floors;
    private float spawnPositionY = 0f;
    private Transform player;

    private void Start()
    {
        floors = new Queue<GameObject>();
        StartCoroutine(InitializePlayer());
    }

    private IEnumerator InitializePlayer()
    {
        while (GameManager.Instance.Player == null)
        {
            Debug.LogWarning("Waiting for player reference...");
            yield return null; 
        }

        player = GameManager.Instance.Player;
        Debug.Log("Player reference found: " + player.name);

        SpawnInitialFloors();
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player is still null in Update.");
            return;
        }

        if (ShouldSpawnNewFloor())
        {
            SpawnFloor();
            RemoveOldFloor();
        }
    }

    private void SpawnInitialFloors()
    {
        for (int i = 0; i < initialFloorCount; i++)
        {
            SpawnFloor();
        }
    }

    private bool ShouldSpawnNewFloor()
    {
        if (player == null)
        {
            Debug.LogError("Player is null inside ShouldSpawnNewFloor().");
            return false;
        }

        return player.position.y > spawnPositionY - (initialFloorCount * floorHeight / 2);
    }

    private void SpawnFloor()
    {
        GameObject newFloor = Instantiate(floorPrefab, new Vector3(1.85f, spawnPositionY, 0), Quaternion.identity);
        floors.Enqueue(newFloor);
        spawnPositionY += floorHeight;
        Debug.Log("Spawned new floor at Y position: " + spawnPositionY);
    }

    private void RemoveOldFloor()
    {
        if (floors.Count > 0)
        {
            GameObject oldFloor = floors.Dequeue();
            Destroy(oldFloor);
            Debug.Log("Removed old floor.");
        }
    }

    private void SpawnEnemy()
    {
        if (player != null) 
        {
            Vector3 spawnPosition = new Vector3(player.position.x, player.position.y + spawnDistance, player.position.z);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Destroy(newEnemy, enemyLifetime);
            Debug.Log("Spawned enemy at position: " + spawnPosition);
        }
        else
        {
            Debug.LogError("Cannot spawn enemy: Player reference is null.");
        }
    }
}
