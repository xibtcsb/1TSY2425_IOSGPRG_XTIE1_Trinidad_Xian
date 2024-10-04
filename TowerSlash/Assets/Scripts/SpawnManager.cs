using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnDistance = 5f;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _enemyLifetime = 5f;

    [Header("Floor Settings")]
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private float _floorHeight = 5f;
    [SerializeField] private int _initialFloorCount = 5;

    private Queue<GameObject> _floors;
    private float _spawnPositionY = 0f;
    private Transform _player;

    private void Awake()
    {
        // Ensure that only one instance of SpawnManager exists
        if (FindObjectsOfType<SpawnManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
    }

    private void Start()
    {
        _floors = new Queue<GameObject>();
        StartCoroutine(InitializePlayer());
    }

    private IEnumerator InitializePlayer()
    {
        while (GameManager.Instance.Player == null)
        {
            yield return null;
        }

        _player = GameManager.Instance.Player;

        SpawnInitialFloors();
        InvokeRepeating(nameof(SpawnEnemy), _spawnInterval, _spawnInterval);
    }

    private void Update()
    {
        if (_player == null)
        {
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
        for (int i = 0; i < _initialFloorCount; i++)
        {
            SpawnFloor();
        }
    }

    private bool ShouldSpawnNewFloor()
    {
        if (_player == null)
        {
            return false;
        }

        return _player.position.y > _spawnPositionY - (_initialFloorCount * _floorHeight / 2);
    }

    private void SpawnFloor()
    {
        GameObject newFloor = Instantiate(_floorPrefab, new Vector3(1.85f, _spawnPositionY, 0), Quaternion.identity);
        _floors.Enqueue(newFloor);
        _spawnPositionY += _floorHeight;
    }

    private void RemoveOldFloor()
    {
        if (_floors.Count > 0)
        {
            GameObject oldFloor = _floors.Dequeue();
            Destroy(oldFloor);
        }
    }

    private void SpawnEnemy()
    {
        if (_player != null)
        {
            Vector3 spawnPosition = new Vector3(_player.position.x, _player.position.y + _spawnDistance, _player.position.z);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            Destroy(newEnemy, _enemyLifetime);
        }
    }
}
